//using BestHTTP;

//namespace Assets.Scripts.Network.Services
//{
//    public static class NetworkCommon
//    {
//        public static bool RequestIsSuccessful(HTTPRequest request, HTTPResponse response, out string errorMessage)
//        {
//            bool status = false;

//            if (response == null || (response.StatusCode != 200 && response.StatusCode != 204))
//            {
//                errorMessage = "Server error";

//                if (response != null)
//                {
//                    switch (response.StatusCode)
//                    {
//                        case 401:
//                            errorMessage = "Unauthorized";
//                            break;
//                        default:
//                            errorMessage = "Server error";
//                            break;
//                    }
//                }
//                else if (request != null && request.Exception != null)
//                {
//                    if (request.Exception.Message.Contains("No connection could be made"))
//                    {
//                        errorMessage = "Please check your internet connection!";
//                    }
//                }

//                // TODO: create global error message handler.
//                // Something like popup that comes at the corner of the screen and disapears after a short period of time.
//                // ref. -> alertify js

//                //errorMessageText.text = errorMessage;
//                //errorMessagePanel.SetActive(true);
//            }
//            else
//            {
//                status = true;
//            }

//            errorMessage = string.Empty;
//            return status;
//        }
//    }
//}
