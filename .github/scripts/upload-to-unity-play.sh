#!/usr/bin/env bash
# Simple script to upload a build zip to Unity Play using the Unity Play API.
# Inputs: $1 = path to zip, $2 = UNITY_PROJECT_ID, $3 = UNITY_PLAY_TOKEN
set -euo pipefail

ZIP_PATH="$1"
PROJECT_ID="$2"
TOKEN="$3"

if [ ! -f "$ZIP_PATH" ]; then
  echo "Build file not found: $ZIP_PATH" >&2
  exit 2
fi

UPLOAD_ENDPOINT="https://play.unity.com/v1/projects/${PROJECT_ID}/builds"

# Create a build record
echo "Creating build record..."
create_response=$(curl -s -X POST "$UPLOAD_ENDPOINT" \
  -H "Authorization: Bearer ${TOKEN}" \
  -H "Content-Type: application/json" \
  -d '{"releaseType":"internal","notes":"Automated build from GitHub Actions"}')

upload_url=$(echo "$create_response" | jq -r '.uploadUrl // empty')
build_id=$(echo "$create_response" | jq -r '.id // empty')

if [ -z "$upload_url" ] || [ -z "$build_id" ]; then
  echo "Failed to create build record: $create_response" >&2
  exit 3
fi

echo "Uploading $ZIP_PATH to upload URL..."
# Upload the zip
curl --progress-bar -X PUT "$upload_url" \
  -H "Content-Type: application/zip" \
  --upload-file "$ZIP_PATH"

# Finalize / publish the build if necessary
echo "Finalizing build..."
finalize_response=$(curl -s -X POST "$UPLOAD_ENDPOINT/${build_id}/publish" \
  -H "Authorization: Bearer ${TOKEN}" \
  -H "Content-Type: application/json" \
  -d '{"isPublic":false}')

echo "Publish response: $finalize_response"

echo "Build uploaded and publish requested. Build id: $build_id"

