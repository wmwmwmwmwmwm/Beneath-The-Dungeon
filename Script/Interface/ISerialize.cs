public interface ISerialize
{
	object SerializeThisObject();
	void DeserializeThisObject(object SavedData);
}
