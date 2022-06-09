using CasseBriques;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public interface IMain
{
    Vector2 GetBounds();
    SpriteBatch GetSpriteBatch();
    void Shake(float pRadius);
    void QuitGame();
    void LaunchGame(bool pBool);
    void SetGameOver(bool pGO);
    bool GetGameOver();
}

public interface ICollider
{
    Vector2 GetPosition();
    Rectangle GetCollRect();
    bool ManageLife();
}
public interface IHole
{
    bool GetState();
    void SetState(bool pState);
}

public interface IManager
{
    bool DeleteObject(ICollider pCollider);
    void LoadFromJson(string pJsonFile);
}

public interface IJson
{
    JsonManager ReadJson(string pFile);
}

public interface ILevel
{
    int GetCurrentLevel();
    void SetCurrentLevel(int pLevel);
}

public interface IImageLoader
{
    Texture2D LoadT2D(string pTex);
}

public interface IAnim
{
    void AddAnimation(int sheetId,int width, int height, Vector2 pos);
    void StartAnimation(Animation pAnim);
    void StopAnim(Animation pAnim);
}

public interface IParticle
{
    void CreateParticle(Vector2 pos);
}

public interface IVisee
{
    void Rotate(float pAngle);
    Vector2 GetAngle();
}