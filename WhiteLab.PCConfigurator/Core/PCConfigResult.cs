namespace WhiteLab.PCConfigurator.Core;

public class PCConfigResult
{
    public bool IsSuccess { get; set; }
    public PCConfig? Config { get; set; }
    public PCConfigError? ConfigError { get; set; }

    public static PCConfigResult Success(PCConfig cnf)
    {
        return new PCConfigResult()
        {
            IsSuccess = true,
            Config = cnf
        };
    }

    public static PCConfigResult Faild(PCConfigError? error)
    {
        return new PCConfigResult()
        {
            IsSuccess = false,
            ConfigError = error
        };
    }

    public static PCConfigResult Set(PCConfig? cnf, PCConfigError? error)
        => cnf != null ? Success(cnf) : Faild(error);

    public static PCConfigResult Set(PCConfig? cnf, string errorMsg = "")
    => cnf != null ? Success(cnf) : Faild(new PCConfigError() { Message = errorMsg});

}
