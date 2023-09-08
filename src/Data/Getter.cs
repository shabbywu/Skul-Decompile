namespace Data;

public delegate T Getter<T>(string key, T defaultValue = default(T));
