# Umbraco Content Files for v7.3+
Provides syncing text based content to physical files. 

This package will create a new document type named 'Content File' if one doesn't already exist. 
You will need to update permissions for the parent document types you would like to create content files within e.g. home.

Only tested in 7.3+ so it currently has a dependency for that version or above.

[![Build status](https://ci.appveyor.com/api/projects/status/3ngp3mxc0cishqkw?svg=true)](https://ci.appveyor.com/project/BarFoo/UmbracoContentFiles)

## Installation
Umbraco content files is available for installation from nuget only at the moment:

| Source      |             |
|-------------|-------------|
| NuGet       | [![NuGet release](https://img.shields.io/nuget/v/UmbracoContentFiles.svg)](https://www.nuget.org/packages/UmbracoContentFiles)|

Install via nuget

`Install-Package UmbracoContentFiles`

## Usages
To allow content editors to save content as physical files on the file system, which can then be accessed directly e.g. `mysite.com/robots.txt`

![alt text](https://raw.githubusercontent.com/BarFoo/UmbracoContentFiles/master/docs/screen1.PNG "Screenshot 1")

The extension of the file will match that used in the content name, for example; `.txt`, `.html`, `.csv` etc. Extensionless files are also supported since version 0.2.3 with the addition of a template.
