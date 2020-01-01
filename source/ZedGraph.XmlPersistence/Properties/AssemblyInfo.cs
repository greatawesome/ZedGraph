using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyDescription("Xml persistence for ZedGraph library")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("0fe7431f-21d0-4b1e-a80c-58ae7fd42dec")]

/* 
 * Format is [Major].[Minor].[2 digit year][3 digit day of year].[2-digit month][2-digit day of month]
 * Here's some power-shell code (can be used in the Package Manager Console too) that gives the last part of the version number:
    $d = Get-Date
    "{0:yy}{1:D3}.{0:MMdd}" -f $d, $d.DayOfYear
*/
[assembly: AssemblyVersion("1.0.20001.0101")]

// Defaults to AssemblyVersion if not given:
//[assembly: AssemblyFileVersion("1.0.0.0")]  
