#!/bin/sh
echo starting...
for file in ./dist/js/app.*.js;
do
  if [ ! -f $file.tmpl.js ]; then
    cp $file $file.tmpl.js
  fi
  envsubst '$VUE_APP_GOODTODO_APIURL' < $file.tmpl.js > $file
done
pushstate-server -d dist -p 80