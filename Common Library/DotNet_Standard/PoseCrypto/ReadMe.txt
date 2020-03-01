<configProtectedData>
    <providers>
      <add name="SampleProvider" type="System.Configuration.RsaProtectedConfigurationProvider, 
           System.Configuration, 
           Version=2.0.0.0, 
           Culture=neutral, 
           PublicKeyToken=b03f5f7f11d50a3a, 
           processorArchitecture=MSIL"
           keyContainerName="SampleRSA"
           useMachineContainer="true" />
    </providers>
</configProtectedData>
<!-- config section 암호화 작업 : $ aspnet_regiis.exe -pef "section name" "webconfig path" -prov "HodGameProvider" -->
<!-- 
assembly load 가 필요한 section의 경우 : 
1. aspnet_regiis.exe 의 경로 C:\Windows\Microsoft.NET\Framework64\v4.0.30319\ 에 dll을 저장하기 위한 폴더 생성. 혹은 빌드된 bin경로를 사용 해도 됨
2. aspnet_regiis.exe가 있는 경로에 aspnet_regiis.exe.config 파일을 생성 후
<configuration>
<runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
        <probing privatePath="dll 경로"/>
    </assemblyBinding>
</runtime>
</configuration>
를 입력.
3. 위의 config section 암호화 작업을 수행하면 암호화 됨.
 -->