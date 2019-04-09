using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web.UI;

// Allgemeine Informationen über eine Assembly werden über die folgenden 
// Attribute gesteuert. Ändern Sie diese Attributwerte, um die Informationen zu ändern,
// die mit einer Assembly verknüpft sind.
[assembly: AssemblyTitle("helferlein")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("helferlein.com")]
[assembly: AssemblyProduct("helferlein.dll")]
[assembly: AssemblyCopyright("Copyright ©  2009-2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Durch Festlegen von ComVisible auf "false" werden die Typen in dieser Assembly unsichtbar 
// für COM-Komponenten. Wenn Sie auf einen Typ in dieser Assembly von 
// COM zugreifen müssen, legen Sie das ComVisible-Attribut für diesen Typ auf "true" fest.
[assembly: ComVisible(false)]

// Die folgende GUID bestimmt die ID der Typbibliothek, wenn dieses Projekt für COM verfügbar gemacht wird
[assembly: Guid("0329e611-8e52-4f4c-8743-ef259b37bc53")]

// Versionsinformationen für eine Assembly bestehen aus den folgenden vier Werten:
//
//      Hauptversion
//      Nebenversion 
//      Buildnummer
//      Revision
//
// Sie können alle Werte angeben oder die standardmäßigen Build- und Revisionsnummern 
// übernehmen, indem Sie "*" eingeben:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("05.00.00")]
[assembly: AssemblyFileVersion("05.00.00")]

// Add assembly reference to allow for WebResource.axd access to JavaScript files
// see: http://aspnet.4guysfromrolla.com/articles/080906-1.aspx
[assembly: WebResource("helferlein.UI.WebControls.Validators.helferlein_Validators.js", "text/javascript")]
[assembly: WebResource("helferlein.UI.WebControls.ftbs.js", "text/javascript")]
