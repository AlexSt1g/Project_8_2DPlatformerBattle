using System;

public interface ICollectable
{
    public event Action<ICollectable, float> Collected;

    public void SetActive(bool activeStatus) { }
}
