# PoseProject
## 머신러닝을 활용한 축구 결과 예측 서비스
* 전세계 150국가의 460개 이상의 리그 결과 예측 

![포세이돈 픽스 Infra](https://user-images.githubusercontent.com/23075175/113105294-65e2fd80-923c-11eb-98ea-70adc741d636.png)
[img1. Server Infrastructure]

### Client
* 경로: MobileApp/PoseSportsPredict
* 개발언어: c#, Json
* 프레임워크: Xamarin.Forms
* AOS / IOS 크로스플랫폼 어플
* 비동기 네트워크 데이터 처리

### Server
* 경로: Web App/PoseSportsWebService
* 개발언어: c#, Json
* 프레임워크: .Net WCF

### DataBase
* RDBMS: My-SQL
* NoSql: Redis

### Management Tool
* 경로: Windows App/SportsAdminTool
* 개발언어: C#
* 프레임워크: WPF
* 전세계 축구데이터 실시간 업데이트
* 유저 관리
* 예측 서버와 통신, 결과 DB에 저장

### Machine Learning
* 경로: MachineLearning/Poseidon_Predictor
* 개발언어: Python
* 프레임워크: TensorFlow
* 전세계 10년치 축구 결과 데이터를 이용해 예측 모델 개발
* 이용 알고리즘: k-최근접 이웃 알고리즘, 랜덤 포레스트, 확률적 경사하강법, 선형회귀

### Predict Server
* 경로: MachineLearning/Poseidon_Predictor_Server
* 개발언어: Python
* 프레임워크: Flask
* 구현된 머신러닝 모델을 이용해 다음 경기 예측데이터 제공

