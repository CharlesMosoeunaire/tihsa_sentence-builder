#!/bin/sh
set -e
if [ -n "$API_BASE_URL" ]; then
  printf '{\n  "apiBaseUrl": "%s"\n}\n' "$API_BASE_URL" > /usr/share/nginx/html/config.json
fi
