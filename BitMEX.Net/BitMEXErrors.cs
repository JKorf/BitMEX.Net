using CryptoExchange.Net.Objects.Errors;

namespace BitMEX.Net
{
    internal static class BitMEXErrors
    {
        public static ErrorMapping RestErrors { get; } = new ErrorMapping([
                new ErrorInfo(ErrorType.InsufficientBalance, false, "Insufficient balance", "19000")
            ], [

                new ErrorEvaluator("HTTPError", (code, msg) => {
                    if (string.IsNullOrEmpty(msg))
                        return ErrorInfo.Unknown;

                    if (msg!.Equals("Invalid API Key."))
                        return new ErrorInfo(ErrorType.Unauthorized, false, "Invalid API key", code);

                    if (msg!.Equals("Invalid symbol"))
                        return new ErrorInfo(ErrorType.UnknownSymbol, false, "Invalid symbol", code);

                    if (msg!.Equals("Invalid orderQty"))
                        return new ErrorInfo(ErrorType.InvalidQuantity, false, "Invalid order quantity", code);

                    if (msg!.Equals("Invalid price"))
                        return new ErrorInfo(ErrorType.InvalidPrice, false, "Invalid order price", code);

                    if (msg.StartsWith("Order quantity must be a multiple of lot size"))
                        return new ErrorInfo(ErrorType.InvalidQuantity, false, "Invalid order quantity, doesn't respect lot size", code);

                    if (msg.StartsWith("Invalid"))
                        return new ErrorInfo(ErrorType.InvalidParameter, false, "Invalid parameter", code);

                    return ErrorInfo.Unknown;
                }),
                new ErrorEvaluator("ValidationError", (code, msg) => {
                    if (string.IsNullOrEmpty(msg))
                        return ErrorInfo.Unknown;

                    if (msg!.Equals("Invalid price"))
                        return new ErrorInfo(ErrorType.InvalidPrice, false, "Invalid order price", code);

                    if (msg.StartsWith("Invalid"))
                        return new ErrorInfo(ErrorType.InvalidParameter, false, "Invalid parameter", code);

                    return ErrorInfo.Unknown;
                })
                ]);

        public static ErrorMapping SocketErrors { get; } = new ErrorMapping([],
            [
               new ErrorEvaluator("400", (code, msg) => {
                    if (string.IsNullOrEmpty(msg))
                        return ErrorInfo.Unknown;

                    if (msg!.StartsWith("Unknown or expired symbol"))
                        return new ErrorInfo(ErrorType.UnknownSymbol, false, "Unknown symbol", code);

                    return ErrorInfo.Unknown;
               })
            ]);
    }
}
