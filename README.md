- 아키텍처 설계 (ICE.cs):

    ICommandExecutor 인터페이스를 만들어 로컬(Python 프로세스)과 원격(SSH.NET) 실행 방식을 통일했습니다.
    RemoteExecutor에 SshClient와 ShellStream을 이용한 실시간 로그 스트리밍 로직을 구현했습니다.
    LocalExecutor에 Process를 이용한 로컬 파이썬 실행 및 로그 파싱 로직을 구현했습니다.



- 로그인 폼 (LoginForm.cs):

    IP, 아이디, 비밀번호 입력, 보기/숨기기(마스킹), 모두 지우기 기능 구현.
    Enter 키로 다음 칸 이동 및 자동 로그인 시도 기능.
    로그인 정보 파일 저장(Persistence) 기능.
    워터마크(Placeholder) 및 에러 메시지 출력 기능.



- UI 연동 (Form1.cs):

    라디오 버튼을 통해 로컬/원격 모드 전환.
    원격 모드 선택 시 로그인 폼 호출 및 성공 시 _executor 자동 세팅.
    취소 시 로컬 모드로 자동 원복하는 안전한 UI 흐름 설계.
    실시간 로그 파싱 및 차트 데이터 갱신 구조 완성.
