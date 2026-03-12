using System;

[Serializable]
// Serializable dictionary item for lists to appear in editor
public class DictionaryItem<K,V>
{
    public K Key;
    public V Value;

    public DictionaryItem(K key, V value)
    {
        Key = key;
        Value = value;
    }
}