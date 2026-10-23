"""Reference one-use handoff store. Not wired into the production portal.

Callables must consult the existing verified-email/current-terms request store.
No HTTP endpoint is implemented here; deploy only after staging integration tests.
"""
import hashlib
import hmac
import re
import secrets
import sqlite3
import time
from contextlib import contextmanager

class Handoffs:
    def __init__(self,path,key,clock=time.time):
        if len(key)<32:raise ValueError('A private server-side key is required')
        self.path,self.key,self.clock=str(path),key,clock
        with self.connection() as c:
            c.execute('CREATE TABLE IF NOT EXISTS launcher_handoffs(digest TEXT PRIMARY KEY,request_ref TEXT NOT NULL,expires REAL NOT NULL,used INTEGER NOT NULL DEFAULT 0)')
    @contextmanager
    def connection(self):
        connection=sqlite3.connect(self.path,timeout=10)
        try:
            with connection:yield connection
        finally:connection.close()
    def digest(self,code):return hmac.new(self.key,code.encode(),hashlib.sha256).hexdigest()
    def issue(self,request_ref,authorized):
        # request_ref is supplied by trusted backend code, never accepted as proof
        # of verification from a browser or launcher request body.
        if not authorized(request_ref):raise PermissionError('Verified current acceptance required')
        code=secrets.token_urlsafe(32)
        with self.connection() as c:c.execute('INSERT INTO launcher_handoffs(digest,request_ref,expires) VALUES(?,?,?)',(self.digest(code),request_ref,self.clock()+600))
        return code
    def redeem(self,code,authorized):
        if not isinstance(code,str) or not re.fullmatch(r'[A-Za-z0-9_-]{43}',code):return None
        with self.connection() as c:
            c.execute('BEGIN IMMEDIATE')
            row=c.execute('SELECT request_ref FROM launcher_handoffs WHERE digest=? AND expires>? AND used=0',(self.digest(code),self.clock())).fetchone()
            if not row or not authorized(row[0]):return None
            c.execute('UPDATE launcher_handoffs SET used=1 WHERE digest=? AND used=0',(self.digest(code),))
            return row[0]
    def purge(self):
        with self.connection() as c:c.execute('DELETE FROM launcher_handoffs WHERE expires<=?',(self.clock(),))
