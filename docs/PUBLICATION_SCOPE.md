# GitHub 공개 범위

현재 선택은 **방식 B + D**, 소스 및 공개 가능한 Scene/설정 중심 저장소입니다. 아직 Git 초기화, 저장소 생성, commit, push 또는 배포를 하지 않았습니다.

## 포함

- Assets/Scripts의 C# 9개와 .meta
- NoPathOut.unity, 실제 참조하는 SampleScene/NavMesh-Navmesh.asset와 .meta
- 자체 Floor/Wall Prefab, 환경 Material, NPC/Player Animator Controller, URP 설정 및 각각의 .meta
- Poly Haven 환경 텍스처 6개, Bowlby One 글꼴과 원본 OFL 고지
- Packages/manifest.json, packages-lock.json, ProjectSettings
- .gitignore, README, 공개 범위·검증 문서

## 제외

| 범위 | 이유 |
| --- | --- |
| .vs/, UserSettings, IDE 프로젝트 파일, UpgradeLog | 로컬 경로·학번과 개발환경 기록. 재생성 가능 |
| Library, Temp, Logs, Obj/obj 및 기타 캐시 | Unity/IDE 생성 파일 |
| Monster/Player 원본 모델·부속 텍스처·Material, Animation FBX | 원본 재배포 권한 근거 부족 |
| Doors 전체, Sounds 전체 | 개별 상품·음원 라이선스 미확인 |
| Web_build | 최소 수정 이전 결과물. 별도 보존 |
| 원본 압축/PPT, 비공개 검수·백업 자료 | 개인 보관 자료. 업로드 대상 밖 |

[PUBLIC_FILES.json](PUBLIC_FILES.json)은 실제 포함 파일을 나열합니다. [EXCLUDED_FILES.json](EXCLUDED_FILES.json)은 외부 에셋 제외 경로를 나열하고 IDE·비공개 파일은 묶음으로 표시하여 개인 경로를 다시 공개하지 않습니다.

공개본에 남긴 모든 Asset과 폴더의 .meta를 보존했습니다. 제외한 원본 에셋의 .meta는 같이 제외했습니다. Scene/Animator의 누락 외부 참조를 복원하려면 원래 파일과 원래 .meta가 필요합니다.

## .gitignore 적용

Unity 캐시·빌드·IDE 파일과 이번 에셋 공개 경계를 지정했습니다. Assets/, Packages/, ProjectSettings/, *.meta, *.asset을 일괄 제외하는 규칙은 없습니다. SampleScene 폴더 아래 실제 NavMesh도 포함됩니다.

Monster/Player 폴더는 프로젝트에서 연결한 Animator Controller 두 개와 .meta만 허용합니다. 다른 경로에 새 에셋을 추가하는 경우까지 자동으로 권리를 검사하는 도구는 아니므로 신규 파일은 다시 확인해야 합니다.

.gitignore는 Git 추적 후보를 제한하는 규칙이며 ZIP 공유나 웹 업로드에 자동 적용되는 필터가 아닙니다. **업로드용으로 선별된 폴더의 내용만 저장소 루트에 사용하고, 전체 PRIVATE 사본이나 원본 ZIP은 올리지 않습니다.** 강제 추가로 제외 규칙을 우회하지 않습니다.

## 용량과 WebGL

기존 Web_build는 107,429,867바이트(약 102.45 MiB)이며 index.html, loader/framework JavaScript, data, wasm, TemplateData 구조입니다. 최소 수정은 반영되지 않았고 이번에 수정·재빌드·배포하지 않았습니다.

Monster FBX는 110,587,680바이트(약 105.46 MiB)로 [GitHub 일반 Git 파일 한도 100 MiB](https://docs.github.com/en/repositories/working-with-files/managing-large-files/about-large-files-on-github)를 넘습니다. 이번에는 재배포 권한 미확인 때문에 제외했으며 Git LFS를 쓰는 것으로 그 문제가 해결되지는 않습니다. 공개본에 LFS 설정을 추가하지 않았습니다.

추후 권리를 확인한 최신 WebGL은 별도로 배포하고 실제 주소를 README에 연결합니다. 그때 호스팅의 MIME·압축 헤더·경로·용량 조건을 새 빌드와 함께 확인합니다. 현재 Web Play는 준비 중입니다.
