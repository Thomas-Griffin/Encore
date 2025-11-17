CI for Unity build & deploy

This project includes a GitHub Actions workflow that builds the Unity project (WebGL) and uploads the build to Unity Play.

Secrets required (Repository secrets):
- UNITY_LICENCE: (optional) Base64-encoded Unity licence file if using Unity activation via licence file. If omitted, the game-ci action will prompt for activation using other supported methods.
- UNITY_PLAY_TOKEN: (required to deploy) Personal access token for Unity Play with rights to upload builds.
- UNITY_PROJECT_ID: (required to deploy) The Unity Play project id (GUID) to which builds will be uploaded.

Workflow location: `.github/workflows/unity-build-and-play.yml`
Helper upload script: `.github/scripts/upload-to-unity-play.sh`

Notes:
- The workflow uses game-ci actions to install Unity and run the build. It reads the editor version from `ProjectSettings/ProjectVersion.txt`.
- The build target is WebGL; adjust `targetPlatform` in the workflow to change platforms.
- The upload script uses the Unity Play API and requires `jq` on the runner (provided on GitHub hosted runners). It expects an uploadUrl and build id in the API response.

Next steps:
- Add repository secrets mentioned above.
- Test by pushing to the `main` branch or running the workflow manually.

