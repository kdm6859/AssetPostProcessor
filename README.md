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

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/ffa4ec29-0a77-481c-9518-81df6cdfbd03/689e1289-587b-4736-a6df-3fe7e8f5fd35/Untitled.png)

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/ffa4ec29-0a77-481c-9518-81df6cdfbd03/5992489d-29fd-40a5-a1a1-5227dcee4e8c/Untitled.png)

- 마우스로 대상 오브젝트(라이트) 클릭 시 프로퍼티를 설정할 수 있는 윈도우창 생성

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/ffa4ec29-0a77-481c-9518-81df6cdfbd03/3461bf87-8983-4600-a6ce-2b5e4348404f/Untitled.png)

### 2. 설치한 오브젝트  Excel 파일로 저장

- 메뉴에 저장버튼을 생성하여 버튼 클릭 시 생성한 오브젝트 Excel 파일로 저장(NPOI 라이브러리)

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/ffa4ec29-0a77-481c-9518-81df6cdfbd03/288bf4a1-ede8-4027-a5a3-de93ed73b21c/Untitled.png)

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/ffa4ec29-0a77-481c-9518-81df6cdfbd03/14ef029b-66ea-43e1-b366-34a50c821e8c/Untitled.png)

### 3. Excel 파일 Import시 오브젝트 설치 및 criptableObject, Json 형식으로 변환

- Excel 파일 Import시 Excel파일의 정보를 읽어 오브젝트 생성

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/ffa4ec29-0a77-481c-9518-81df6cdfbd03/72698be9-34a1-474c-b426-e3935e775003/Untitled.png)

- Excel 파일의 정보를 criptableObject, Json 형식으로 변환

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/ffa4ec29-0a77-481c-9518-81df6cdfbd03/b8566d51-4560-49d5-a2e8-64eeb7f73002/Untitled.png)

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/ffa4ec29-0a77-481c-9518-81df6cdfbd03/4808e882-e06c-41da-a206-3057d983b176/Untitled.png)
