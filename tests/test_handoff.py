import concurrent.futures, pathlib,sys,tempfile,unittest
sys.path.insert(0,str(pathlib.Path(__file__).resolve().parents[1]/'server'))
from handoff import Handoffs
TEST_ROOT=pathlib.Path(__file__).resolve().parents[1]/'out'
TEST_ROOT.mkdir(exist_ok=True)
class Tests(unittest.TestCase):
 def test_verification_expiry_replay_and_current_terms(self):
  with tempfile.TemporaryDirectory(dir=TEST_ROOT) as d:
   clock=[1000];s=Handoffs(pathlib.Path(d)/'test.sqlite3',b'x'*32,lambda:clock[0])
   with self.assertRaises(PermissionError):s.issue('ref',lambda r:False)
   code=s.issue('ref',lambda r:True)
   self.assertIsNone(s.redeem(code,lambda r:False))
   self.assertEqual(s.redeem(code,lambda r:True),'ref');self.assertIsNone(s.redeem(code,lambda r:True))
   code=s.issue('ref',lambda r:True);clock[0]+=601;self.assertIsNone(s.redeem(code,lambda r:True));s.purge()
   self.assertIsNone(s.redeem('invalid',lambda r:True))
 def test_only_one_concurrent_redemption(self):
  with tempfile.TemporaryDirectory(dir=TEST_ROOT) as d:
   s=Handoffs(pathlib.Path(d)/'test.sqlite3',b'x'*32);code=s.issue('ref',lambda r:True)
   with concurrent.futures.ThreadPoolExecutor(max_workers=6) as pool:results=list(pool.map(lambda _:s.redeem(code,lambda r:True),range(6)))
   self.assertEqual(results.count('ref'),1)
if __name__=='__main__':unittest.main()
