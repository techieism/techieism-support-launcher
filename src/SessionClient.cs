// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
namespace Techieism.Support {
 public static class SessionClient {
  public const string SupportPage="https://help.techieism.com/support.html";
  public const string RedemptionEndpoint="https://help.techieism.com/support-api/launcher/redeem";
  public const int MaxBytes=8*1024*1024;
  public static bool ValidCode(string code){return code!=null && Regex.IsMatch(code,"\\A[A-Za-z0-9_-]{43}\\z");}
  public static byte[] ValidatePayload(Stream stream,string digest){
   if(digest==null||!Regex.IsMatch(digest,"\\A[a-fA-F0-9]{64}\\z"))throw new InvalidDataException("Invalid support response.");
   using(var output=new MemoryStream()){
    var chunk=new byte[16384];int n;
    while((n=stream.Read(chunk,0,chunk.Length))>0){if(output.Length+n>MaxBytes)throw new InvalidDataException("Support response exceeds the size limit.");output.Write(chunk,0,n);}
    byte[] bytes=output.ToArray();
    if(bytes.Length<64||bytes[0]!=77||bytes[1]!=90)throw new InvalidDataException("Invalid support executable.");
    int pe=BitConverter.ToInt32(bytes,60);
    if(pe<64||pe>bytes.Length-4||bytes[pe]!=80||bytes[pe+1]!=69||bytes[pe+2]!=0||bytes[pe+3]!=0)throw new InvalidDataException("Invalid support executable header.");
    string actual=BitConverter.ToString(SHA256.Create().ComputeHash(bytes)).Replace("-","");
    if(!String.Equals(actual,digest,StringComparison.OrdinalIgnoreCase))throw new InvalidDataException("The support download is damaged.");
    return bytes;
   }
  }
  public static byte[] Redeem(string code){
   if(!ValidCode(code))throw new ArgumentException("Paste the one-time support code from the verified support page.");
   ServicePointManager.SecurityProtocol=SecurityProtocolType.Tls12;
   var request=(HttpWebRequest)WebRequest.Create(RedemptionEndpoint);
   request.Method="POST";request.ContentType="application/json";request.AllowAutoRedirect=false;request.Timeout=30000;request.ReadWriteTimeout=30000;request.Proxy=WebRequest.DefaultWebProxy;
   request.Headers["Origin"]="https://help.techieism.com";
   byte[] body=Encoding.UTF8.GetBytes("{\"handoff_code\":\""+code+"\"}");request.ContentLength=body.Length;
   using(var stream=request.GetRequestStream())stream.Write(body,0,body.Length);
   try{using(var response=(HttpWebResponse)request.GetResponse()){
    if(response.StatusCode!=HttpStatusCode.OK||response.ContentType.Split(';')[0]!="application/octet-stream"||response.ContentLength>MaxBytes)throw new InvalidDataException("The support server did not return an Assistant.");
    using(var stream=response.GetResponseStream())return ValidatePayload(stream,response.Headers["X-Techieism-Assistant-SHA256"]);
   }}catch(WebException){throw new InvalidOperationException("Support authorization is unavailable, expired or already used. Verify email again if this is a new session. Do not disable Windows security protection.");}
  }
 }
}
