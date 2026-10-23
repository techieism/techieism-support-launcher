// SPDX-License-Identifier: MIT
using System;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
[assembly:AssemblyTitle("Techieism Support Launcher")]
[assembly:AssemblyProduct("Techieism Support Launcher")]
[assembly:AssemblyCompany("Techieism India LLP")]
[assembly:AssemblyCopyright("Copyright (c) 2026 Techieism India LLP")]
[assembly:AssemblyVersion("0.1.0.0")]
[assembly:AssemblyFileVersion("0.1.0.0")]
namespace Techieism.Support {
 class Window:Form {
  TextBox code=new TextBox{Left=20,Top=175,Width=450,UseSystemPasswordChar=true,MaxLength=43};
  Button start=new Button{Text="Start verified support",Left=20,Top=215,Width=220};
  Label status=new Label{Left=20,Top=260,Width=460,Height=85};
  public Window(){
   Text="Techieism Support Launcher — preview";Width=510;Height=400;FormBorderStyle=FormBorderStyle.FixedDialog;MaximizeBox=false;
   Controls.Add(new Label{Text="Customer-initiated support only. Verify your email and accept the support terms on our website. Paste the one-time support code below. No password or email OTP belongs in this box. This preview is not enabled for live support.",Left=20,Top=15,Width=460,Height=85});
   var website=new Button{Text="Open verified support website",Left=20,Top=110,Width=300};website.Click+=(s,e)=>Process.Start(new ProcessStartInfo(SessionClient.SupportPage){UseShellExecute=true});Controls.Add(website);
   Controls.Add(new Label{Text="One-time support code",Left=20,Top=150,Width=300});Controls.Add(code);Controls.Add(start);Controls.Add(status);
   start.Click+=async(s,e)=>{start.Enabled=false;string handoff=code.Text.Trim();code.Clear();try{await Task.Run(()=>Launch(handoff));status.Text="The Assistant has closed. End Support closes server authorization permanently; a brief connection loss has a five-minute recovery window.";}catch(Exception ex){status.Text=ex.Message;}finally{start.Enabled=true;}};
  }
  static void Launch(string code){
   if(TrustedAssistant.ApprovedSignerThumbprint.Length!=40)throw new InvalidOperationException("Preview only: signer verification and server handoff integration must be completed before release.");
   byte[] payload=SessionClient.Redeem(code);string folder=Path.Combine(Path.GetTempPath(),"TechieismSupport-"+Guid.NewGuid().ToString("N"));
   var security=new DirectorySecurity();security.SetAccessRuleProtection(true,false);
   foreach(var sid in new[]{WellKnownSidType.BuiltinAdministratorsSid,WellKnownSidType.LocalSystemSid})security.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(sid,null),FileSystemRights.FullControl,InheritanceFlags.ContainerInherit|InheritanceFlags.ObjectInherit,PropagationFlags.None,AccessControlType.Allow));
   Directory.CreateDirectory(folder,security);
   try{string exe=Path.Combine(folder,"TechieismSupportAssistant.exe");File.WriteAllBytes(exe,payload);TrustedAssistant.Verify(exe);using(var p=Process.Start(new ProcessStartInfo(exe){UseShellExecute=false,WorkingDirectory=folder}))p.WaitForExit();}
   finally{try{Directory.Delete(folder,true);}catch{/* May remain after an interrupted session; removal instructions are documented. */}}
  }
 }
 static class Program {
  [STAThread]static void Main(){if(!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)){MessageBox.Show("Windows administrator approval is required.");return;}Application.EnableVisualStyles();Application.Run(new Window());}
 }
}
