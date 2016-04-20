
public interface IBuilder<T>
{
    T Build();
}

public interface IBuilder<T, K1>
{
    T Build(K1 p1);
}

public interface IBuilder<T, K1, K2>
{
    T Build(K1 p1, K2 p2);
}
