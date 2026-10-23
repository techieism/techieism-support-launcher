// SPDX-License-Identifier: MIT
using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
namespace Techieism.Support {
 public static class TrustedAssistant {
  // Release blocker: populate from an independently verified upstream Assistant
  // certificate. Never accept a certificate supplied by the redemption response.
  public const string ApprovedSignerThumbprint="";
  [StructLayout(LayoutKind.Sequential,CharSet=CharSet.Unicode)] struct FileInfo {public uint size;[MarshalAs(UnmanagedType.LPWStr)]public string path;public IntPtr file,subject;}
  [StructLayout(LayoutKind.Sequential,CharSet=CharSet.Unicode)] struct TrustData {public uint size;public IntPtr policy,sip;public uint ui,revocation,choice;public IntPtr file;public uint action;public IntPtr state,url;public uint flags,context;}
  [DllImport("wintrust.dll",ExactSpelling=true,CharSet=CharSet.Unicode)] static extern int WinVerifyTrust(IntPtr window,[In] ref Guid action,[In,Out] ref TrustData data);
  public static void Verify(string path){
   if(ApprovedSignerThumbprint.Length!=40)throw new InvalidOperationException("This preview is not enabled for live remote support. The approved Assistant signer has not been configured.");
   var file=new FileInfo{size=(uint)Marshal.SizeOf(typeof(FileInfo)),path=path};IntPtr pointer=Marshal.AllocHGlobal(Marshal.SizeOf(typeof(FileInfo)));Marshal.StructureToPtr(file,pointer,false);
   var data=new TrustData{size=(uint)Marshal.SizeOf(typeof(TrustData)),ui=2,revocation=1,choice=1,file=pointer,action=1,flags=0x80};var action=new Guid("00AAC56B-CD44-11d0-8CC2-00C04FC295EE");
   try{
    if(WinVerifyTrust(new IntPtr(-1),ref action,ref data)!=0)throw new InvalidOperationException("Windows could not verify the Assistant signature. Contact Helpdesk.");
    var certificate=new X509Certificate2(X509Certificate.CreateFromSignedFile(path));
    if(!String.Equals(certificate.Thumbprint,ApprovedSignerThumbprint,StringComparison.OrdinalIgnoreCase))throw new InvalidOperationException("The Assistant publisher does not match this release.");
   }finally{data.action=2;WinVerifyTrust(new IntPtr(-1),ref action,ref data);Marshal.DestroyStructure(pointer,typeof(FileInfo));Marshal.FreeHGlobal(pointer);}
  }
 }
}
