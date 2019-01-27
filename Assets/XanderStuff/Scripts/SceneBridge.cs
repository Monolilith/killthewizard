
public class SceneBridge {

    private static SceneBridge instance;
	public static SceneBridge Instance
    {
        get
        {
            if (instance == null)
                instance = new SceneBridge();

            return instance;
        }
    }



    public string characterName;

}
