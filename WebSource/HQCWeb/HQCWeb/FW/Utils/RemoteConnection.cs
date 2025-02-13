using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HQCWeb.FMB_FW.Utils
{
    public static class RemoteConnection
    {
        #region 원격 연결에 대한 설정 및 해제를 위한 변수

        private static object _lockObject = new object();

        public static Dictionary<string, int> connectedState = new Dictionary<string, int>();
        public static List<string> targetRootPathList = new List<string>();
        
        #endregion


        #region 원격 네트워크 연결관련 API 및 상수, 변수 선언
        /// <summary>
        /// WNetUseConnection - dwFlags(0 or CONNECT_UPDATE_PROFILE)
        /// 0일 경우는 연결해제만 하지만 CONNECT_UPDATE_PROFILE의 경우는 연결해제 후 드라이브를 완전히 삭제한다.
        /// </summary>
        private const uint CONNECT_UPDATE_PROFILE = 0x1;

        /// <summary>
        ///  NETRESOURCE - dwScope : 탐색할 네트워크리소스 범위
        /// </summary>
        private const uint RESOURCE_CONNECTED = 0x1;   // 현재 연결된 리소스
        private const uint RESOURCE_GLOBALNET = 0x2;   // 모든 리소스
        private const uint RESOURCE_REMEMBERED = 0x3;   // 로그온할 때마다 자동으로 연결되는 리소스

        /// <summary>
        /// NETRESOURCE - dwType : 리소스 종류
        /// </summary>
        private const uint RESOURCETYPE_ANY = 0x0;    // 모든 리소스
        private const uint RESOURCETYPE_DISK = 0x1;    // 디스크
        private const uint RESOURCETYPE_PRINT = 0x2;    // 프린터

        /// <summary>
        /// NETRESOURCE - dwDisplayType : 검색된 리소스의 타입
        /// </summary>
        private const uint RESOURCEDISPLAYTYPE_DOMAIN = 0x1;   // 도메인
        private const uint RESOURCEDISPLAYTYPE_GENERIC = 0x0;   // 모든 리소스
        private const uint RESOURCEDISPLAYTYPE_SERVER = 0x2;   // 서버
        private const uint RESOURCEDISPLAYTYPE_SHARE = 0x3;   // 공유포인트

        /// <summary>
        /// NETRESOURCE - dwUsage : dwScope값이 RESOURCE_GLOBALNET인 경우만 의미가 있음.
        /// </summary>
        private const uint RESOURCEUSAGE_CONNECTABLE = 0x1; // 공유 포인트
        private const uint RESOURCEUSAGE_CONTAINER = 0x2; // 컨테이너 리소스

        /// <summary>
        /// API 반환값 상수
        /// </summary>
        private const int NO_ERROR = 0;
        private const int ERROR_ACCESS_DENIED = 5;
        private const int ERROR_ALREADY_ASSIGNED = 85;
        private const int ERROR_BAD_DEV_TYPE = 66;
        private const int ERROR_BAD_DEVICE = 1200;
        private const int ERROR_BAD_NET_NAME = 67;
        private const int ERROR_BAD_NETPATH = 53;
        private const int ERROR_BAD_PROFILE = 1206;
        private const int ERROR_BAD_PROVIDER = 1204;
        private const int ERROR_BUSY = 170;
        private const int ERROR_CANCELLED = 1223;
        private const int ERROR_CANNOT_OPEN_PROFILE = 1205;
        private const int ERROR_DEVICE_ALREADY_REMEMBERED = 1202;
        private const int ERROR_EXTENDED_ERROR = 1208;
        private const int ERROR_INVALID_ADDRESS = 487;
        private const int ERROR_INVALID_PARAMETER = 87;
        private const int ERROR_INVALID_PASSWORD = 86;
        private const int ERROR_NO_NET_OR_BAD_PATH = 1203;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct NETRESOURCE
        {
            public uint dwScope;
            public uint dwType;
            public uint dwDisplayType;
            public uint dwUsage;
            public string lpLocalName;
            public string lpRemoteName;
            public string lpComment;
            public string lpProvider;
        }

        // Net Drive 연결
        [DllImport("mpr.dll", CharSet = CharSet.Auto)]
        private static extern int WNetUseConnection(
                                                    IntPtr hwndOwner
                                                   , [MarshalAs(UnmanagedType.Struct)] ref NETRESOURCE lpNetResource
                                                   , string lpPassword
                                                   , string lpUserID
                                                   , uint dwFlags
                                                   , StringBuilder lpAccessName
                                                   , ref int lpBufferSize
                                                   , out uint lpResult
                                                   );

        // Net Drive 해제
        [DllImport("mpr.dll", EntryPoint = "WNetCancelConnection2", CharSet = CharSet.Auto)]
        private static extern int WNetCancelConnection2A(string lpName, int dwFlags, int fForce);

        #endregion











        public static string SetRemoteConnection(string connectionPath, string userID, string userPwd)
        {
            int capacity = 256;

            uint resultFlags = 0;
            uint flags = 0;

            string resultMessage = null;

            if (connectionPath != "" || connectionPath != string.Empty)
            {
                lock (RemoteConnection._lockObject)
                {
                    // 기존에 열린 연결이 있으면 연결을 사용중인 수만 증가시킨다.
                    if (RemoteConnection.connectedState.ContainsKey(connectionPath) == true)
                    {
                        //액세스 거부 오류로 인해 서버킬 때 한번 연결해두면끌때까지 유지
                        //RemoteConnection.connectedState[connectionPath] = RemoteConnection.connectedState[connectionPath] + 1;
                        resultMessage = string.Empty;
                    }
                    // 기존에 열린 연결이 없으면 새로운 연결을 설정한다.
                    else
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder(capacity);
                        NETRESOURCE ns = new NETRESOURCE()
                        {
                            dwScope = RESOURCE_GLOBALNET
                                                              ,
                            dwType = RESOURCETYPE_DISK
                                                              ,
                            dwDisplayType = RESOURCEDISPLAYTYPE_GENERIC
                                                              ,
                            dwUsage = RESOURCEUSAGE_CONNECTABLE
                                                              ,
                            lpLocalName = null // 로컬 드라이브명(null 이면 자동)
                                                              ,
                            lpRemoteName = connectionPath
                                                              ,
                            lpProvider = null
                        };

                        // result 변수 값이 0 이면 오류가 없는 것이며, 0 이 아닌 값은 오류가 발생했음을 알리는 오류 코드이다.
                        // 명시적으로 로컬 드라이브를 지정하고 싶다면 NETRESOURCE 구조체의 lpLocalName 필드에 Z:, Y: 등의 값을 설정할 수 있다.
                        // 이 경우, 이미 네트워크 드라이브 사용 중이라면 오류 코드 85 를 반환할 것이다.
                        // 만약, 사용 가능한 로컬 드라이브 (Z, Y, X, W, 등의 순서) 이름의 선택을 시스템에게 맞기고 싶다면
                        // flags 값에 0x80 을 주면 시스템이 적절한 로컬 드라이브 이름을 선택해 줄 것이다.
                        // 그리고 그 드라이브 이름은 StringBuilder 를 통해 값이 반환된다.
                        // flags 가 0x80 이 아닌 값이 사용되면 StringBuilder는 일반적으로 공유 폴더의 UNC 이름을 반환한다.
                        // 따라서 capacity 값은 공유 폴더의 경로를 담을 수 있도록 충분히 주어야 한다. 그렇지 않으면 오류 코드 234를 반환할 것이다.
                        int result = WNetUseConnection(IntPtr.Zero, ref ns, userPwd, userID, flags, sb, ref capacity, out resultFlags);

                        switch (result)
                        {
                            case NO_ERROR:
                                RemoteConnection.connectedState.Add(connectionPath, 1);
                                resultMessage = string.Empty;
                                targetRootPathList.Add(connectionPath);
                                break;

                            case ERROR_ALREADY_ASSIGNED:
                                resultMessage = "지정한 로컬 드라이브 이름이 이미 네트워크 리소스에 연결되어 있습니다.";
                                break;

                            case ERROR_BAD_NET_NAME:
                                resultMessage = "리소스 이름이 유효하지 않거나 명명된 리소스를 찾을 수 없습니다.";
                                break;

                            case ERROR_BAD_NETPATH:
                                resultMessage = "네트워크 경로를 찾을 수 없습니다.";
                                break;

                            case ERROR_BAD_DEV_TYPE:
                                resultMessage = "로컬 장치 유형 및 네트워크 리소스 유형이 일치하지 않습니다.";
                                break;

                            case ERROR_ACCESS_DENIED:
                                resultMessage = "네트워크 리소스에 액세스할 권한이 없습니다.";
                                break;

                            case ERROR_BAD_DEVICE:
                                resultMessage = "로컬 드라이브 이름이 유효하지 않습니다.";
                                break;

                            case ERROR_BAD_PROVIDER:
                                resultMessage = "lpProvider 구성원에 의해 지정된 값이 공급자와 일치하지 않습니다.";
                                break;

                            case ERROR_BUSY:
                                resultMessage = "라우터 또는 공급자가 사용 중이므로 초기화 할 수 있습니다. 발신자가 다시 시도해야합니다.";
                                break;

                            case ERROR_CANCELLED:
                                resultMessage = "네트워크 리소스 공급자 중 하나의 대화 상자 또는 호출 된 리소스에 의해 사용자가 연결 시도를 취소했습니다.";
                                break;

                            case ERROR_CANNOT_OPEN_PROFILE:
                                resultMessage = "시스템이 영구 연결을 처리하기 위해 사용자 프로파일을 열 수 없습니다.";
                                break;

                            case ERROR_DEVICE_ALREADY_REMEMBERED:
                                resultMessage = "lpLocalName 구성원에 의해 지정된 장치에 대한 항목 이 이미 사용자 프로필에 있습니다.";
                                break;

                            case ERROR_EXTENDED_ERROR:
                                resultMessage = "네트워크 관련 오류가 발생했습니다. 오류에 대한 설명을 얻으려면 WNetGetLastError 함수를 호출하십시오.";
                                break;

                            case ERROR_INVALID_ADDRESS:
                                resultMessage = "호출자가 액세스 할 수없는 버퍼에 대한 포인터를 전달했습니다.";
                                break;

                            case ERROR_INVALID_PARAMETER:
                                resultMessage = "이 오류는 다음 조건 중 하나의 결과입니다.\r\n"
                                              + "  1. lpRemoteName의 멤버는 NULL . 또한 lpAccessName 은 NULL 이 아니지만 lpBufferSize 가 NULL 이거나 0을 가리 킵니다.\r\n"
                                              + "  2. dwType의 멤버도 RESOURCETYPE_DISK도 RESOURCETYPE_PRINT입니다. 또한 CONNECT_REDIRECT는 dwFlags에 설정 되고 lpLocalName 은 NULL 이거나 로컬 장치의 경로 재 지정이 필요한 네트워크에 대한 연결입니다.";
                                break;

                            case ERROR_INVALID_PASSWORD:
                                resultMessage = "지정된 비밀번호가 유효하지 않으며 CONNECT_INTERACTIVE 플래그가 설정되지 않았습니다.";
                                break;

                            case ERROR_NO_NET_OR_BAD_PATH:
                                resultMessage = "네트워크 구성 요소가 시작되지 않았거나 지정된 자원 이름이 인식되지 않아서 조작을 완료 할 수 없습니다.";
                                break;

                            default:
                                resultMessage = string.Format("WNetUseConnection 호출시 미리 정의되지 않은 에러가 발생하였습니다. 에러코드:{0}", result);
                                break;
                        }
                    }
                }
            }
            else
            {
                resultMessage = "경로가 입력되지 않았습니다.";
            }

            if (resultMessage.IsNullOrEmpty() == false)
            {
                resultMessage = string.Format("Fail RemoteConnection Path=[{0}], UserID=[{1}], ErrorMessage=[{2}]", connectionPath, userID, resultMessage);
            }

            return resultMessage;
        }
    }
}
