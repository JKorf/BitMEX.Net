using CryptoExchange.Net.Objects.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMEX.Net
{
    internal static class BitMEXErrors
    {
        public static ErrorCollection Errors { get; } = new ErrorCollection([
                new ErrorInfo(ErrorType.InsufficientBalance, false, "Insufficient balance", "19000")
            ], [

                new ErrorEvaluator("HTTPError", (code, msg) => {
                    if (string.IsNullOrEmpty(msg))
                        return ErrorInfo.Unknown;

                    if (msg!.Equals("Invalid symbol"))
                        return new ErrorInfo(ErrorType.UnknownSymbol, false, "Invalid symbol", code);

                    if (msg!.Equals("Invalid orderQty"))
                        return new ErrorInfo(ErrorType.InvalidQuantity, false, "Invalid order quantity", code);

                    if (msg!.Equals("Invalid price"))
                        return new ErrorInfo(ErrorType.InvalidPrice, false, "Invalid order price", code);

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
    }
}
