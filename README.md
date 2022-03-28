# portfolio-magic_cube

## 제작 환경 : Unity, C#

## 주요 구현 내용 :    


1. 씬 이동 및 캐릭터 선택
    - [싱글톤을 사용한 씬로드 매니저](https://github.com/ChangminYoo/portfolio-magic_cube/blob/main/Assets/03.%20Scripts/Manager/SceneLoadManager.cs) 구현.
    - 커스터 마이징 구현.


2. 기본 이동, 공격, 점프, 대쉬 구현
    - 조이스틱, 버튼 조작을 [커맨드 패턴을 이용한 입력 시스템](https://github.com/ChangminYoo/portfolio-magic_cube/blob/main/Assets/03.%20Scripts/Manager/InputManager.cs)으로 캐릭터 구현.  

    - 캐릭터 hp와 스태미너 구현.
    - [오브젝트 풀](https://github.com/ChangminYoo/portfolio-magic_cube/blob/main/Assets/03.%20Scripts/Manager/ObjectPoolManager.cs) 활용한 공격 구현.

3. [몬스터](https://github.com/ChangminYoo/portfolio-magic_cube/blob/main/Assets/03.%20Scripts/Actor/Monster.cs) 기본 FSM 구현
    - 상태에 따른 idle, chase, attack, dead 구현.

