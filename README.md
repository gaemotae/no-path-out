# NO PATH OUT

**Unity / C#로 구현한 1인칭 미로 탈출 게임.** 몬스터의 순찰과 추격을 피해 출구에 도달하는 게임입니다. **FSM·NavMesh 이동·FOV/Raycast 감지**, **CharacterController 이동**, 게임 상태와 NPC 행동의 연결을 중심으로 구현했습니다.

**Unity 6000.0.59f2에서 실행 검증 완료.** 제작자가 외부 에셋을 보유한 전체 최소 수정본을 직접 실행해 확인했습니다. 이 저장소는 외부 모델·애니메이션·음원을 제외한 **소스 및 Scene/설정 공개본**이므로 다운로드만으로 같은 게임을 바로 실행할 수는 없습니다.

## 개발 정보

| 항목 | 내용 |
| --- | --- |
| 엔진 / 언어 | Unity 6000.0.59f2 / C# |
| 주요 Unity 기능 | NavMeshAgent, CharacterController, Physics Raycast/Linecast, Trigger Collider, Animator, AudioSource, uGUI |
| 패키지 | AI Navigation 2.0.9, Universal RP 17.0.4, uGUI 2.0.0 |
| 입력 | 기존 Input Manager의 Input.GetAxis / Input.GetKey 사용. Active Input Handling은 Both |
| 핵심 Scene | Assets/Scenes/NoPathOut.unity |

개발 기간과 개인/팀 구분은 자료만으로 확정하지 않아 기재하지 않았습니다. 구현 설명은 당시 교수 제출 제작 보고서, 현재 코드·Scene, 제작자의 실행 확인을 근거로 작성했습니다.

## 주요 구현

| 구현 | 동작과 기술적 포인트 | 코드 |
| --- | --- | --- |
| NPC FSM + NavMesh | Patrol에서 WayPoint를 선택하고 Chase에서 플레이어로 목적지를 갱신합니다. 감지가 끊기면 Search로 이동하고 탐색 목적지 도착 후 순찰로 복귀합니다. 경로 이동은 Unity NavMeshAgent를 사용합니다. | [WatcherFSM.cs](Assets/Scripts/WatcherFSM.cs), [FSM.cs](Assets/Scripts/FSM.cs) |
| FOV / 벽 차폐 감지 | 감지 거리 → 시야각 → Wall LayerMask Raycast 순서로 검사한 CanSeePlayer 결과를 상태 전환에 사용합니다. | [WatcherFSM.cs](Assets/Scripts/WatcherFSM.cs) |
| 플레이어 이동 / 시점 | CharacterController에 입력 방향·걷기/달리기 속도·중력을 적용합니다. 마우스 수평 회전은 플레이어, 수직 회전은 카메라에 적용합니다. | [PlayerController.cs](Assets/Scripts/PlayerController.cs), [MouseController.cs](Assets/Scripts/MouseController.cs) |
| 포획 / 게임 결과 | 거리 Catch와 Trigger Catch가 공통 벽 차폐 검사를 통과해야 합니다. Playing 중 최초 Clear 또는 GameOver만 확정하며, 종료 후 NPC의 추가 결과 요청을 무시합니다. | [WatcherFSM.cs](Assets/Scripts/WatcherFSM.cs), [CatchTrigger.cs](Assets/Scripts/CatchTrigger.cs), [ExitTrigger.cs](Assets/Scripts/ExitTrigger.cs), [UIManager.cs](Assets/Scripts/UIManager.cs) |
| Animation / Audio | NPC의 Patrol/Search와 Chase에 따라 Animator bool을 갱신합니다. 실제 이동과 상태에 따라 발소리 간격을 선택하고 추격음의 시작·종료를 연결합니다. 플레이어도 이동/달리기를 Animator와 발소리에 연결합니다. | [WatcherFSM.cs](Assets/Scripts/WatcherFSM.cs), [PlayerController.cs](Assets/Scripts/PlayerController.cs), [AudioController.cs](Assets/Scripts/AudioController.cs) |

### 게임 상태와 NPC 행동

Start 화면에서 클릭하면 Playing으로 전환합니다. NPC는 Patrol → Chase → Search → Patrol 흐름으로 동작하며, Search 중 다시 감지하면 Chase로 돌아갑니다. 유효한 포획은 GameOver, ExitZone 진입은 Clear를 요청합니다. `UIStateControl`은 Playing 상태에서 들어온 첫 결과만 받아 결과 화면과 시간 정지를 적용합니다.

결과 화면에서 클릭하면 `Time.timeScale`을 복원하고 현재 Scene을 다시 로드합니다. 새 Start 화면에서 한 번 더 클릭해 다음 플레이를 시작합니다.

## 핵심 문제 해결

### 당시 제작 과정

1. **NavMesh 도착 판정 개선** — Vector3.Distance 기반 도착 판정으로 Patrol에서 멈추는 문제를 경험했습니다. 경로 계산 중인지 `pathPending`으로 확인하고, `remainingDistance`를 `stoppingDistance`와 허용 거리 기준으로 비교하는 방식을 적용했습니다. 현재 Patrol/Search 코드에도 유지되어 있습니다.
2. **중복 회전 제어 제거** — MouseController 회전과 PlayerController LookAt의 충돌을 확인해 LookAt을 제거했습니다. 현재 이동은 PlayerController, 시점 회전은 MouseLook이 담당합니다.
3. **Trigger Collider로 Catch 보완** — 가까이 접근해도 거리 판정만으로 포획이 안정적이지 않아 Trigger 경로를 추가했습니다. 현재 거리 판정과 `OnTriggerEnter` / `OnTriggerStay`가 함께 남아 있습니다.

### 2026 포트폴리오 공개 준비 과정

당시 구조를 보존한 최소 수정이며, 과거 제작 당시 해결한 사례와 구분합니다.

1. **GameOver 반복 처리 방지** — UI의 Playing 여부를 확인해 이미 끝난 게임의 추가 결과 처리를 막았습니다.
2. **Clear / GameOver 결과 중복 방지** — `UIStateControl.TrySetResult()`에서 최초 결과만 확정하도록 모았습니다.
3. **벽 너머 포획 방지** — 거리/접촉 Catch가 공통 `TryCatch()`를 거치며 기존 Wall 마스크로 `Physics.Linecast`를 수행합니다. 가까운 뒤쪽 플레이어도 잡을 수 있도록 FOV 시야각 제한은 적용하지 않습니다.
4. **대각선 이동 속도 보정** — 입력 벡터를 `Vector3.ClampMagnitude(..., 1f)`로 제한했습니다. 걷기·달리기 속도와 중력 값은 유지했습니다.

## AI 활용 및 기여 범위

당시 교수 제출 자료에 AI 도움을 받은 범위를 기록했습니다.

| 구분 | 실제 활용과 작업 |
| --- | --- |
| AI의 구조·코드 도움 | FSM 안에서 NavMeshAgent/SetDestination을 활용하는 구조, Patrol/Chase/Search 목적지 갱신과 전환, CanSeePlayer 결과 활용, remainingDistance/pathPending, Trigger Catch 보완, FSM–Animation 연결, Footstep 및 UI On/Off 코드 |
| 직접 적용·조정 | 미로·조명 Scene 구성, NavMesh 구성, Inspector 파라미터 조정, 오브젝트·컴포넌트 연결, 코드 적용·수정, 게임 흐름 통합 |
| 직접 확인 | 당시 도착 판정·회전 충돌·포획 문제 재현과 해결 방식 판단, 적용 후 플레이 확인, 2026 최소 수정본의 Unity 실행 검증 |

수업 예제와 이전 학기 코드도 기반으로 활용했습니다. 이동·UI·오디오 등 모든 코드를 독자적으로 처음부터 설계했다고 주장하지 않습니다. 외부 캐릭터·클립·음원 제작도 구현 기여에 포함하지 않습니다. 2026 최소 수정과 공개 문서 정리에도 AI 코딩 도구를 활용했고, 최소 수정본은 제작자가 Unity에서 직접 확인했습니다.

## 프로젝트 구조

| 경로 | 내용 |
| --- | --- |
| Assets/Scripts/ | C# 9개와 각 .meta. FSM.cs는 WatcherFSM의 기반 클래스 |
| Assets/Scenes/NoPathOut.unity | 실제 플레이 Scene과 Script 연결 |
| Assets/Scenes/SampleScene/NavMesh-Navmesh.asset | 실제 Scene이 사용하는 베이크된 NavMesh. 폴더명과 관계없이 유지 |
| Assets/Prefab/ | 미로 Floor / Wall Prefab |
| Assets/Monster/, Assets/Player/ | 공개본에는 연결한 Animator Controller와 .meta만 포함 |
| Assets/Materials/, Assets/textures/, Assets/Fonts/ | 환경 Material, 확인된 CC0 텍스처, OFL 글꼴과 고지 |
| Assets/Settings/ | URP / Volume 설정 |
| Packages/, ProjectSettings/ | 패키지 버전, 입력, 레이어, 빌드 Scene 등 |
| docs/ | 외부 리소스·공개 범위, 검증 기록과 파일 목록 |

`SoundGameManager.cs`는 현재 Scene/Prefab에서 참조되지 않는 보존 코드입니다. 이 파일의 코루틴 크로스페이드는 플레이 기능으로 소개하지 않습니다. 원본 CP949 인코딩을 유지하므로 일부 웹 뷰어에서 한글 주석 표시가 다를 수 있습니다. `MouseController.cs`의 클래스는 `MouseLook`, `UIManager.cs`의 클래스는 `UIStateControl`이며 기존 연결을 보존했습니다.

## 조작법

| 입력 | 동작 |
| --- | --- |
| Left Click | Start / Restart. 결과 화면에서는 Scene을 다시 로드 |
| WASD | Move |
| Left Shift | Run |
| Mouse | Look |

## 실행 방법

**공개본의 목적은 코드와 Scene 구성 검토입니다.** 모델·Animation FBX·Door 리소스·오디오와 해당 .meta가 제외되어 Scene/Animator에 누락 참조가 남습니다. 공개본만 열어 동일한 플레이가 된다고 보장하지 않습니다.

외부 에셋의 사용 권한과 원래 리소스를 갖춘 전체 프로젝트 기준:

1. Unity Hub에서 **Unity 6000.0.59f2**로 Assets, Packages, ProjectSettings가 있는 루트를 엽니다.
2. Package Manager 복원을 기다립니다. 기존 패키지 버전, Input Handling(Both), Player/WayPoint 태그와 Wall 레이어를 유지합니다.
3. 공개본에서 복원할 경우 [외부 리소스 안내](docs/EXTERNAL_ASSETS.md)를 확인합니다. 필요한 파일과 **원래 .meta**를 함께 복원해야 기존 GUID 연결을 유지할 수 있습니다. 동명 에셋 재다운로드만으로 GUID·FBX 내부 참조·설정이 복원되지는 않습니다.
4. Assets/Scenes/NoPathOut.unity를 열고 Play를 누릅니다. 베이크된 NavMesh를 유지하고 Start 화면에서 클릭합니다.

### 실행 검증

제작자가 Unity 6000.0.59f2에서 전체 최소 수정본의 **C# Compile Error 0**, Start, WASD/Shift/Mouse Look, Patrol/Chase/Search, Catch, 벽 사이 Catch 방지, Exit Clear, 결과 후 재시작과 수정 후 게임 흐름을 확인했습니다. 기존 경고 2개는 남아 있으며 종류는 별도 기록되지 않았습니다.

이번 공개 정리에서는 C# 9개와 Scene·NavMesh를 변경하지 않았습니다. 공개 설정에 남아 있던 학번만 빌드 명칭 메타데이터 세 필드에서 게임명으로 치환했습니다. 공개용 일부 파일만으로 다시 실행 검증한 것은 아니며, FPS·성능 수치·모든 엣지케이스의 검증 결과는 제시하지 않습니다. [검증 범위](docs/VALIDATION.md)

## Web Play

**[Play NO PATH OUT in browser](https://gaemotae.github.io/no-path-out/)**

Unity 6000.0.59f2의 최신 최소 수정본으로 새 Web 빌드를 생성해 `gh-pages` 브랜치에 배포했습니다. 제작자 PC의 Unity `Build And Run`에서 브라우저 플레이를 확인했고, GitHub Pages 공개 주소의 정상 로딩도 확인했습니다. 기존 제작 당시 `Web_build`는 사용하지 않았습니다.

`main`에는 외부 모델·Animation FBX·Door·음원 원본을 포함하지 않으며, Web Play는 별도의 빌드 산출물만 `gh-pages`에서 제공합니다.

## External Assets / Credits

| 포함 리소스 | 출처 / 라이선스 |
| --- | --- |
| Concrete Floor Worn 02의 4K Diffuse / ARM / Normal DX | [Poly Haven 개별 자산](https://polyhaven.com/a/concrete_floor_worn_02), Dimitrios Savva, [CC0](https://polyhaven.com/license) |
| Worn Plaster Wall의 4K Diffuse / ARM / Normal DX | [Poly Haven 개별 자산](https://polyhaven.com/a/worn_plaster_wall), Dimitrios Savva, [CC0](https://polyhaven.com/license) |
| Bowlby One Regular | Vernon Adams, SIL Open Font License 1.1. [동봉 원문](Assets/Fonts/Bowlby_One/FontLicense.txt) 유지 |

당시 제출 자료에는 Skin & Animation: Mixamo / Unity Asset Store, Audio: Freesound로 기재되어 있습니다. 개별 원본 재배포 권리를 확인할 수 없는 모델·클립·Door·음원은 `main` 소스 공개본에서 제외했습니다. Web Play는 원본 파일 공개가 아닌 빌드된 게임 산출물 형태로 제공합니다. [파일별 분류와 근거](docs/EXTERNAL_ASSETS.md)

## License

C# 소스는 구현 검토를 위해 공개하는 범위이며 별도의 오픈소스 재사용 라이선스는 아직 지정하지 않았습니다. 수업 예제 등 기반 코드의 권리까지 포괄해 MIT 같은 단일 라이선스를 부여하지 않습니다.

CC0 텍스처와 OFL 글꼴은 각각의 라이선스를 따릅니다. 글꼴의 저작권·라이선스 고지를 함께 유지합니다. Unity 패키지는 각 패키지의 조건을 따르며, 제외한 외부 에셋의 권리를 이 저장소가 부여하지 않습니다.
