# MagicCube

## 제작 환경 : Unity, C#

## 주요 구현 내용 :    


1. 씬 이동 및 캐릭터 선택
    - [싱글톤을 사용한 씬로드 매니저](https://github.com/ChangminYoo/portfolio-magic_cube/blob/main/Assets/03.%20Scripts/Manager/SceneLoadManager.cs) 구현.
    - 시네머신 카메라 사용하여 커스터 마이징 구현.

    ![select](https://user-images.githubusercontent.com/22344444/164166054-1a240e66-8d12-466a-b7e9-699f68ecf8b7.gif)

2. 기본 이동, 공격, 점프, 대쉬 구현
    - 조이스틱, 버튼 조작을 [커맨드 패턴을 이용한 입력 시스템](https://github.com/ChangminYoo/portfolio-magic_cube/blob/main/Assets/03.%20Scripts/Manager/InputManager.cs)으로 캐릭터 구현.  

    - 캐릭터 hp와 스태미너 구현.
    - [오브젝트 풀](https://github.com/ChangminYoo/portfolio-magic_cube/blob/main/Assets/03.%20Scripts/Manager/ObjectPoolManager.cs) 활용한 공격 구현.

    ![move](https://user-images.githubusercontent.com/22344444/160320422-41a2efe2-c4b1-437f-a60e-bd57e924a2fb.gif)
3. [몬스터](https://github.com/ChangminYoo/portfolio-magic_cube/blob/main/Assets/03.%20Scripts/Actor/Monster.cs) 기본 FSM 구현
    - 상태에 따른 idle, chase, attack, dead 구현.

    ![monster](https://user-images.githubusercontent.com/22344444/160320111-48e338fa-2983-442d-9bdc-f8d043c65ad8.gif)
    
4. 기타 구현 : 맵, 파티클 이펙트, 무기 교체, 시간/점수 표시및 게임 오버

## 미로게임
- [Hunt_And_Kill 알고리즘](https://github.com/ChangminYoo/portfolio-magic_cube/blob/main/Assets/03.%20Scripts/Object/MazeGenerator.cs) 사용한 미로 구현
- [A Star 알고리즘](https://github.com/ChangminYoo/portfolio-magic_cube/blob/main/Assets/03.%20Scripts/MazePathFinder.cs) 사용한 미로 길찾기 구현


![maze1](https://user-images.githubusercontent.com/22344444/163316943-8988c604-fd75-46ec-b79e-263ff6f2a5d5.png)

## 앞으로 추가할 내용
- 시네머신, 타임라인 이용한 간단한 연출
- 스킬및 능력 추가
- 게임 모드 추가
- 포톤 서버추가
