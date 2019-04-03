using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Resources;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyDescription("ZedGraph is a class library, user control, and web control for .net, written in C#, for drawing 2D Line, Bar, and Pie Charts. It features full, detailed customization capabilities, but most options have defaults for ease of use.")]
[assembly: AssemblyTrademark( "" )]
[assembly: AssemblyCulture( "" )]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible( false )]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid( "a552bf32-72a3-4d27-968c-72e7a90243f2" )]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("6.4.19092.0403")]
[assembly: AssemblyFileVersion("6.4.19092.0403")]
//[assembly: AllowPartiallyTrustedCallers ]
[assembly: NeutralResourcesLanguageAttribute( "" )]

/* 
Here's some power-shell code (can be used in the Package Manager Console too) that gives the last part of the version number:

$d = Get-Date
"{0:yy}{1:D3}.{0:MMdd}" -f $d, $d.DayOfYear


Here's some code that works in the new C# Interactive window too:
Console.WriteLine(string.Format("{0:yy}{1:D3}.{0:MMdd}", DateTime.Now, DateTime.Now.DayOfYear));
*/
