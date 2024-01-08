## 프로젝트 소개

- Editor를 커스텀 하여 유니티의 Editor 기능을 직접 구현
- 클릭만으로 오브젝트를 설치
- NPOI 라이브러리를 이용하여 설치된 오브젝트를 Excel 파일로 저장하는 기능 구현
- AssetPostprocessor를 이용하여 오브젝트 정보를 저장한 Excel파일을 유니티에 임포트 할 때 ScriptableObject, Json 형식으로 변환 및 오브젝트 설치

## 시연 영상

https://youtu.be/MiLL36TP6oo

## 목차

1. Custom Editor로 오브젝트(라이트) 설치 기능 구현
2. 설치한 오브젝트  Excel 파일로 저장
3. Excel 파일 Import시 오브젝트 설치 및 criptableObject, Json 형식으로 변환

### 1. Custom Editor로 오브젝트(라이트) 설치 기능 구현

- Scene에서 ctrl키를 누른 상태로 마우스 왼쪽 버튼 클릭 시 오브젝트 생성
- 오브젝트에 핸들을 생성하여 위치 조정 및 회전 조정 가능

![Untitled (1)](https://github.com/kdm6859/AssetPostProcessor/assets/64892955/7bc985b0-b14c-4dd3-adbb-055cbf70e419)

![Untitled (1)](https://github.com/kdm6859/AssetPostProcessor/assets/64892955/4e367b8d-e124-45bd-b759-e2b9798fe438)


- 마우스로 대상 오브젝트(라이트) 클릭 시 프로퍼티를 설정할 수 있는 윈도우창 생성

![Untitled (2)](https://github.com/kdm6859/AssetPostProcessor/assets/64892955/4e3abef6-a417-4fe6-83ca-18a5f90f2910)

- A키로 오브젝트 그룹 생성
- C키으로 오브젝트 그룹 변경 가능
- delete 키로 오브젝트 삭제, shift + delete 로 오브젝트 그룹 삭제

### 2. 설치한 오브젝트  Excel 파일로 저장

- 메뉴에 저장버튼을 생성하여 버튼 클릭 시 생성한 오브젝트 Excel 파일로 저장(NPOI 라이브러리)

![Untitled (3)](https://github.com/kdm6859/AssetPostProcessor/assets/64892955/6a5c2c29-1c3f-4d2d-9d39-2408cc220f88)

![Untitled (4)](https://github.com/kdm6859/AssetPostProcessor/assets/64892955/d80d1498-dcab-49dc-83f1-0fc3b9685fe3)

### 3. Excel 파일 Import시 오브젝트 설치 및 criptableObject, Json 형식으로 변환

- Excel 파일 Import시 Excel파일의 정보를 읽어 오브젝트 생성

![Untitled (5)](https://github.com/kdm6859/AssetPostProcessor/assets/64892955/7e32867f-7e45-45b0-968c-393e2580faed)

- Excel 파일의 정보를 criptableObject, Json 형식으로 변환

![제목 없음](https://github.com/kdm6859/AssetPostProcessor/assets/64892955/3bbe2bd0-c040-480f-bdc3-1a6cc88a4422)

