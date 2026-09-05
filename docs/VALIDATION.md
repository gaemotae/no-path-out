# 검증 범위와 보존 기록

## Unity 실행 확인

제작자는 **Unity 6000.0.59f2**에서 NoPathOut_Nexon2026_MinimalFix 전체 프로젝트를 직접 실행해 확인했습니다. 이는 제작자의 보고이며 공개 정리를 수행한 AI 도구가 Unity를 실행한 결과가 아닙니다.

| 항목 | 제작자가 확인한 결과 |
| --- | --- |
| 프로젝트 / 컴파일 | 정상 오픈, C# Compile Error 0, 기존 Warning 2개 |
| 시작 / 조작 | Start, WASD, Shift, Mouse Look 정상 |
| NPC | Patrol / Chase / Search 정상 |
| Catch | 포획 정상, 벽을 사이에 둔 포획 방지 정상 |
| 결과 / 재시작 | Exit Clear, GameOver/Clear 후 재시작 정상 |
| 최소 수정 이후 | GameOver/Clear/Catch/대각선 이동 관련 동작과 전체 게임 흐름 정상 |

경고 두 개의 정확한 메시지, FPS·성능 수치, 모든 경계 상황 및 정해진 횟수의 반복 시험 결과는 제공되지 않아 검증 완료 범위를 확대하지 않습니다.

## 시점 구분

1. 당시 제작: 교수 제출 보고서에 기록된 도착 판정, 회전 충돌, Trigger 보완 사례.
2. 2026 최소 수정: 종료 결과 처리·벽 차폐·대각선 이동을 고친 뒤 AI 도구가 정적 검수 수행.
3. 제작자 확인: 전체 최소 수정본을 Unity 6000.0.59f2에서 직접 실행해 정상 동작 확인.
4. 이번 공개 준비: 별도 사본의 공개 범위와 문서 정리. 새 Unity 실행과 WebGL 빌드는 수행하지 않음.

이전 보고서의 ‘Unity 실행 미수행’은 2번 시점에 해당합니다. 현재 상태는 3번의 제작자 확인을 반영합니다.

## 보존 및 변경 범위

- 이전 최소 수정 기록의 SHA-256과 복구한 최소 수정본 258개 파일이 모두 일치했습니다.
- C# 9개, Scene, .meta, 자체 Prefab, Animator, NavMesh 및 패키지·엔진 버전은 원본 바이트를 유지했습니다.
- 공개 사본의 ProjectSettings/ProjectSettings.asset에서 productName, metroPackageName, metroApplicationDescription의 학번만 NoPathOut으로 바꿨습니다. 다른 설정은 유지했습니다.
- .gitignore, README, 공개 범위 문서를 추가했습니다. 외부 에셋은 전체 준비용 사본에 보존하고 업로드용 폴더에서는 제외했습니다.
- 원본 ZIP, 교수 제출 PPT, 검증된 최소 수정본, 기존 Web_build는 수정하지 않았습니다.

## 코드 공개 품질

| 분류 | 확인 사항 / 처리 |
| --- | --- |
| 반드시 수정할 C# | 이번 공개 정리 기준 새로 발견한 항목 없음. 추가 코드 수정 없음 |
| 공개 설정 정리 | 제품명 메타데이터 3필드의 학번만 공개 사본에서 치환 |
| 선택 수정 | 미사용 SoundGameManager.cs의 CP949 한글 주석. UTF-8 변환은 별도 승인 후 가능하며 이번에는 유지 |
| 선택 수정 | 종료 성공 후 Debug.Log 두 곳. 결과 중복 방지 경로 뒤에서 출력하며 개인정보 없음. 이번에는 유지 |
| 그대로 유지 | 파일명/클래스명 차이와 GUID 연결, FSM/NavMesh 구조, lastSeenPosition, 속도·오디오·입력·Spawn/Animator·Scene 디자인 |

8개 C#은 UTF-8(BOM 차이 있음), SoundGameManager.cs는 CP949이며 줄바꿈도 유지했습니다. SoundGameManager는 현재 Scene/Prefab에서 미연결이며 활성 크로스페이드 기능으로 설명하지 않습니다. FSM.cs는 직접 부착되지 않아도 WatcherFSM의 기반 클래스로 사용됩니다.

검토한 공개 소스·설정에서 실제 자격증명으로 보이는 API Key, Token, 비밀번호 또는 로컬 사용자 절대경로를 발견하지 못했습니다. 빈 설정 키와 글꼴 저작권 고지의 제작자 이메일은 비밀정보로 분류하지 않았습니다. 패턴 검사와 파일 검토 결과이며 모든 가능한 비밀정보의 부재를 보증하지는 않습니다.

## 현재 코드와 설명의 일치

- Scene의 플레이어 걷기/달리기는 3 / 4.5, NPC NavMeshAgent 속도는 3.5입니다. NPC가 항상 더 빠르다고 설명하지 않습니다.
- Search 목표는 감지가 끊기는 순간 읽은 player.position입니다. 마지막으로 보였던 위치를 매 프레임 정밀 기록한다고 표현하지 않습니다.
- 거리 Catch와 Trigger Catch가 함께 남아 있고 공통 Wall Linecast에는 FOV 각도 제한이 없습니다.
- 현재 walkClip/runClip에는 같은 Walking_2.wav가 연결됩니다. 서로 다른 걷기/달리기 원본 음원을 적용했다고 표현하지 않습니다.
- 외부 Animation을 Animator와 연결한 작업이며 캐릭터 모델링·클립 원본 제작을 기여로 주장하지 않습니다.

실행 검증을 마친 것은 전체 최소 수정본입니다. 에셋을 제외한 공개본 자체는 누락 참조가 있으므로 소스·구성 검토용이며 실제 플레이 링크는 새 WebGL 단계에서 준비합니다.
