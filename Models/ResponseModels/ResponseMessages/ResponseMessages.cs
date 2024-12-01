namespace test2.Models.ResponseModels
{
    public class ResponseMessages
    {
        public static readonly Dictionary<int, string> ErrorMessagesDict = new Dictionary<int, string>
        {

            //application errors
            { 0001, "Aplication error put en contact with the administrator" },
            { 0002, "{0} Database error do somthing about it " },
            
            
        
        };


       

       
         public enum ErrorCode
        {
            application = 1001,
            database = 1002,
           
        }

        // Método para obtener un mensaje de error con un marcador de posición
        public static string GetErrorMessage(ErrorCode errorCode, string fieldName = "")
        {
            string message = ErrorMessagesDict.ContainsKey((int)errorCode) ? ErrorMessagesDict[(int)errorCode] : "Error desconocido.";
            return string.IsNullOrEmpty(fieldName) ? message : string.Format(message, fieldName);
        }
    }
}