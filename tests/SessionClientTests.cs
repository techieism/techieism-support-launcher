// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Security.Cryptography;
using Techieism.Support;
class Tests {
 static int checks;
 static void Assert(bool value){checks++;if(!value)throw new Exception("Test failed: "+checks);}
 static string Hash(byte[] b){return BitConverter.ToString(SHA256.Create().ComputeHash(b)).Replace("-","");}
 static void Reject(Action action){bool rejected=false;try{action();}catch(InvalidDataException){rejected=true;}Assert(rejected);}
 static void Main(){
  Assert(SessionClient.ValidCode(new string('A',43)));Assert(!SessionClient.ValidCode(new string('A',42)));Assert(!SessionClient.ValidCode(new string('A',43)+"\n"));Assert(!SessionClient.ValidCode("https://evil.example/"));Assert(!SessionClient.ValidCode(null));
  var bytes=new byte[128];bytes[0]=77;bytes[1]=90;BitConverter.GetBytes(80).CopyTo(bytes,60);bytes[80]=80;bytes[81]=69;
  Assert(SessionClient.ValidatePayload(new MemoryStream(bytes),Hash(bytes)).Length==128);
  Reject(()=>SessionClient.ValidatePayload(new MemoryStream(bytes),new string('0',64)));
  Reject(()=>SessionClient.ValidatePayload(new MemoryStream(bytes),null));
  var bad=(byte[])bytes.Clone();bad[0]=0;Reject(()=>SessionClient.ValidatePayload(new MemoryStream(bad),Hash(bad)));
  BitConverter.GetBytes(Int32.MaxValue).CopyTo(bad,60);bad[0]=77;Reject(()=>SessionClient.ValidatePayload(new MemoryStream(bad),Hash(bad)));
  var large=new byte[SessionClient.MaxBytes+1];Reject(()=>SessionClient.ValidatePayload(new MemoryStream(large),Hash(large)));
  Assert(SessionClient.RedemptionEndpoint=="https://help.techieism.com/support-api/launcher/redeem");
  Assert(TrustedAssistant.ApprovedSignerThumbprint.Length==0);bool blocked=false;try{TrustedAssistant.Verify("not-a-file");}catch(InvalidOperationException){blocked=true;}Assert(blocked);
  Console.WriteLine("PASS: "+checks+" checks; no network calls, customer data or administrator execution.");
 }
}
