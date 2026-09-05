# External Assets / 공개 범위

2026-09-05 기준으로 당시 제출 자료, 실제 파일·동봉 고지, 아래 공식 페이지를 대조했습니다. 사이트 이름이나 Mixamo 리깅 표식만으로 개별 자산의 원본 재배포 권리를 확정하지 않았습니다.

**A: 포함 가능 / B: 포함 보류 / C: 제외 권고.** B와 C는 모두 이번 업로드 대상에서 제외합니다.

| 리소스 | 실제 파일 / 범위 | 판정 | 근거와 처리 |
| --- | --- | --- | --- |
| Monster FBX | Assets/Monster/Ch10_nonPBR.fbx 및 Ch10_nonPBR.fbm/ | C | Mixamo / Asset Store 통합 출처만 기록되어 원본 모델의 권리까지 입증되지 않습니다. 원본 FBX·부속 텍스처 제외 |
| Player FBX | Assets/Player/Remy.fbx 및 Remy.fbm/ | C | 게임 이용 안내를 캐릭터 원본 재배포 허가로 확대하지 않음. FBX·부속 텍스처 제외 |
| Animation FBX | Monster의 Old Man Idle.fbx, Walking (2).fbx, Running (1).fbx; Player의 Idle (1).fbx, Walking (1).fbx, Running.fbx | C | 외부 클립 원본 6개 제외. 프로젝트에서 연결한 Animator Controller는 유지 |
| Door Asset | Assets/Doors/ 전체 | B | 상품명·제작자·개별 라이선스 미확인. 모델·텍스처뿐 아니라 동봉 Material/Prefab도 제외 |
| BGM | Assets/Sounds/BGM.mp3 | B | 개별 Freesound 원문·작가·라이선스 버전 미확인으로 제외 |
| Chase Audio | Assets/Sounds/Chase.aiff | B | 개별 Freesound 라이선스 미확인으로 제외 |
| Footstep Audio | Assets/Sounds/Walking_2.wav, Walking_1.wav, Running.wav | B | 파일별 라이선스 미확인으로 모두 제외. 현재 Scene은 Player/NPC 걷기·달리기에 Walking_2를 연결하며 나머지 두 파일은 현재 사용이 확인되지 않음 |
| 환경 Texture | Assets/textures/의 concrete_floor_worn_02 및 worn_plaster_wall 4K JPG 6개 | A | 제출 자료의 Poly Haven 출처에 더해 공식 개별 자산명, 동일 파일명·해상도·종류 다운로드와 CC0 확인. Diffuse/ARM/Normal DX 파일 및 .meta 포함 |
| Font | Assets/Fonts/Bowlby_One/BowlbyOne-Regular.ttf | A | 동봉 FontLicense.txt에 Vernon Adams, SIL OFL 1.1 명시. 글꼴·원문 고지·.meta 함께 포함 |

Monster/Player 원본과 함께 생성·입수된 Material도 보수적으로 제외했습니다. 제외 에셋의 .meta는 함께 제외하며, **포함한 모든 Asset과 폴더의 기존 .meta는 유지**합니다. 전체 준비용 사본에서는 어떤 외부 에셋도 삭제하지 않았습니다.

환경 Texture의 A 판정은 위 두 개별 자산의 공식 다운로드 이름·규격을 현재 파일과 대응시킨 결과입니다. 원격 원본과의 전체 SHA-256 일치 검증까지 수행한 것은 아닙니다. 모델에 딸린 .fbm 텍스처와 Door 텍스처는 이 CC0 판정에 포함되지 않습니다.

## 근거

| 출처 | 확인한 내용 |
| --- | --- |
| [Poly Haven License](https://polyhaven.com/license) | 자산의 CC0 및 재배포 허용. 사이트 로고·미리보기 이미지까지 같은 허가로 취급하지 않음 |
| [Concrete Floor Worn 02](https://polyhaven.com/a/concrete_floor_worn_02), [Worn Plaster Wall](https://polyhaven.com/a/worn_plaster_wall) | 각 4K Diffuse/ARM/Normal DX 다운로드, Dimitrios Savva, CC0 확인 |
| [Adobe Mixamo FAQ](https://helpx.adobe.com/creative-cloud/faq/mixamo-faq.html) | 캐릭터·애니메이션의 게임 이용 안내. 원본 FBX/클립 공유에 대한 별도 허가로 해석하지 않음 |
| [Unity Asset Store EULA](https://unity.com/legal/as-terms) | 2.2.1은 게임 등에 포함된 이용·배포를 다룸. 원본을 무제한 공개하는 허가가 아니며 개별 자산의 별도 조건도 확인 필요 |
| [Freesound FAQ](https://freesound.org/help/faq/) | 파일별 CC0, CC BY, CC BY-NC 등 조건이 다르고 예전 Sampling+도 존재. 사이트 표기만으로 일괄 허용 불가 |
| [FontLicense.txt](../Assets/Fonts/Bowlby_One/FontLicense.txt), [SIL OFL 원문](https://openfontlicense.org/open-font-license-official-text/) | 글꼴 재배포 시 저작권·라이선스 고지 유지. 이름과 라이선스 변경 없음 |

## 실행 의존성과 공개 전략

현재는 **방식 B + D: 소스와 공개 가능한 Scene/설정을 포함하고 권리 미확인 외부 자산을 제외**합니다. Scene, NavMesh, 자체 Floor/Wall Prefab, Animator Controller 구성은 유지하므로 외부 원본에 대한 누락 참조가 남습니다. 공개본만으로 같은 게임이 바로 실행된다고 보장하지 않습니다.

제작자는 전체 최소 수정본을 보존합니다. 사용 권한이 있는 전체 프로젝트 복원 시에는 원래 파일과 **원래 .meta**가 함께 필요합니다. 동명 파일 재다운로드만으로 GUID, FBX 내부 오브젝트, 설정이 일치하지는 않습니다. 정확한 상품명이 없는 모델·Door·음원에는 임의 다운로드 링크를 만들지 않았습니다.

향후 방식 C(소스 + 별도 WebGL)를 완성하려면 모델·클립·Door의 게임 배포 권한과 음원별 라이선스·표시 의무를 확인해야 합니다. WebGL이나 Git LFS를 사용해도 라이선스 문제는 해결되지 않습니다.

## 추가로 필요한 정보

- Monster/Player의 취득 페이지·원작자·라이선스. Mixamo에 업로드한 모델이라면 업로드 이전 출처도 필요
- Animation 취득 경로·적용 조건, Door의 정확한 상품명·상품 페이지·라이선스
- BGM/Chase/발소리의 Freesound sound ID 또는 원문 URL, 작가, 라이선스 이름·버전 및 크레딧 요구사항
- 수업 기반 코드에 별도 공개 조건이 있었다면 교재/강의 예제 출처와 조건. 단일 오픈소스 라이선스를 지정하기 전에 구분

위 자산 정보는 제외 원본을 다시 포함하거나 게임 빌드를 배포하기 전 확인해야 합니다. 현재 업로드본에는 제외 원본이나 교수 제출 PPT를 포함하지 않습니다. [외부 에셋 제외 경로와 기타 제외 범위](EXCLUDED_FILES.json)
