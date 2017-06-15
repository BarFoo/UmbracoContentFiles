# Umbraco Text Files (Alpha)
Text file content nodes that are synced to physical text files.

This package will create a new document type named 'Text File' with an alias of 'textFile' if one doesn't already exist. You will need to update
permissions for the parent nodes you would like to create text files within e.g. home.

Only tested in 7.3+ so it has a dependency for that version or above.

Install via nuget

`Install-Package UmbracoTextFiles`

WARNING: Only install this after you have installed Umbraco. I will sort out a workaround soon, the try catch isn't enough.

More to come soon...