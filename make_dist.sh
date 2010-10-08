#!/bin/bash

VERSION=$1
DISTDIR=MarkEmptyDirs_$VERSION

mkdir -p dist/$DISTDIR/bin
mkdir -p dist/$DISTDIR/docs/wiki
cp docs/* dist/$DISTDIR/docs > /dev/null
pushd docs/wiki
hg archive ../../dist/$DISTDIR/docs/wiki
popd
cp COPYING dist/$DISTDIR
cp src/DJ.App.MarkEmptyDirs/bin/Debug/*.exe dist/$DISTDIR/bin
cp src/DJ.App.MarkEmptyDirs/bin/Debug/*.dll dist/$DISTDIR/bin

cd dist
zip -r -9 $DISTDIR.zip $DISTDIR/

