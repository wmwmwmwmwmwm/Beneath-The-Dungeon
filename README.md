# 1 인개발게임 - Beneath The Dungeon

3D 타일식 던전을 탐험하는 턴제 로그라이크 모바일 게임입니다.


[![BTD 플레이영상](http://img.youtube.com/vi/y6IwfYzzMvU/0.jpg)](https://youtu.be/y6IwfYzzMvU) 


***




# 씬

**Scenes\000First (0:00)** : 시작 씬으로, 매니저 클래스를 로드합니다.

**Scenes\100StartMenu (0:00 ~ 0:10)** : 메인 메뉴 화면입니다.

**Scenes\300MainDungeon (0:10 ~ 2:10)** : 던전 씬으로, 층이나 테마가 바뀔 때는 같은 씬을 로드하면서 해당 층 정보에 따라 맵을 바꿉니다.





***





# 주요 클래스

**Script\Common\AudioManager.cs** : 배경음과 효과음 출력을 담당합니다.<br>

**Script\Common\CommonUI.cs** : 모든 씬에서 쓰일 수 있는 팝업 등의 UI를 담당합니다.<br>

**Script\Common\DBManager.cs** : 데이터 테이블과 그래픽 리소스를 관리합니다. 씬을 이동할 때마다 필요한 리소스를 Addressable로 불러옵니다.<br>

**Script\Common\EncountEventManager.cs** : 전투, 아이템 획득, 계단과 같은 특수 타일 이벤트를 관리합니다.<br>

**Script\Common\UICanvas.cs** : 던전 탐험 중(전투가 아닐 때)에 보이는 UI를 관리합니다.<br>

**Script\Common\DungeonController.cs** : 던전의 정보(현재 층, 맵과 이벤트 생성 등)를 관리합니다.<br>

**Script\Common\SaveManager.cs** : 저장 및 로드를 담당합니다.<br>




***
