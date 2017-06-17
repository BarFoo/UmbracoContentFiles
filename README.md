# Umbraco Content Files (Alpha)
This package will create a new document type named 'Content File' if one doesn't already exist. 
You will need to update permissions for the parent document types you would like to create content files within e.g. home.

Only tested in 7.3+ so it has a dependency for that version or above.

[![Build status](https://ci.appveyor.com/api/projects/status/3ngp3mxc0cishqkw?svg=true)](https://ci.appveyor.com/project/BarFoo/UmbracoContentFiles)

## Installation
Umbraco content files is available for installation from nuget only at the moment:

| Source      |             |
|-------------|-------------|
| NuGet       | [![NuGet release](https://img.shields.io/nuget/v/UmbracoContentFiles.svg)](https://www.nuget.org/packages/UmbracoContentFiles)|

Install via nuget

`Install-Package UmbracoContentFiles`

## Use Cases
To allow content editors to save file content as actual physical files on the file system, which can then be accessed directly e.g. `mysite.com/robots.txt`

![alt text](https://raw.githubusercontent.com/BarFoo/UmbracoContentFiles/master/docs/screen1.PNG "Screenshot 1")

More information to come soon...
