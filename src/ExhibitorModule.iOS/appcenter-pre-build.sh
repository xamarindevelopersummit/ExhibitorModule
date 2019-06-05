#!/usr/bin/env bash
#
# For Xamarin Android or iOS, change the version name located in AndroidManifest.xml and Info.plist. 
# AN IMPORTANT THING: YOU NEED DECLARE VERSION_NAME ENVIRONMENT VARIABLE IN APP CENTER BUILD CONFIGURATION.

if [ ! -n "$VERSION_NAME" ]
then
    echo "You need define the VERSION_NAME variable in App Center"
    exit
fi

INFO_PLIST_FILE=$APPCENTER_SOURCE_DIRECTORY/build/BuildTemplateInfo.plist

if [ -e "$INFO_PLIST_FILE" ]
then
    echo "Updating version name to $VERSION_NAME.$APPCENTER_BUILD_ID $INFO_PLIST_FILE in Info.plist"
    plutil -replace CFBundleShortVersionString -string $VERSION_NAME'.'$APPCENTER_BUILD_ID $INFO_PLIST_FILE
    plutil -replace CFBundleVersion -string $VERSION_NAME'.'$APPCENTER_BUILD_ID $INFO_PLIST_FILE

    echo "File content:"
    cat $INFO_PLIST_FILE
fi

