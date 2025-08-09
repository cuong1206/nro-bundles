using System;

public static class Babyshark
{
    // Constants
    private static int CLOUD_Y_OFFSET1 = 20;
    private static int CLOUD_Y_OFFSET2 = 60;
    private static int MAX_HEIGHT = 1000;
    private static int MIN_HEIGHT = -1000;
    private static int GOKU_Y_MIN = -70;
    private static int GOKU_Y_MAX = 70;
    private static int SCREEN_MARGIN = 10;
    private static int RESPAWN_OFFSET = 100;
    private static int MOVEMENT_SPEED = 1;


    

    // Touch control handling
    private static int lastTouchX = 0;
    private static int lastTouchY = 0;
    private static Boolean isControlling = false;
    private static int sharkX = 0;
    private static int sharkY = 0;
    private static Boolean isMovingRight = true; // Biến kiểm tra hướng di chuyển
    public static void paintbabyshark(mGraphics g, int x, int y, int w)
    {
        int speed = MOVEMENT_SPEED;

        // Khởi tạo vị trí ban đầu nếu chưa được đặt
        if (sharkX == 0 && sharkY == 0)
        {
            sharkX = x;
            sharkY = (GameScr.cmy >> 3) + y; // Giữ cá mập ở độ cao cố định
        }

        // Logic di chuyển tự động theo đường thẳng
        if (isMovingRight)
        {
            sharkX += speed;
            if (sharkX > x + w) // Khi đến vị trí x + w thì đổi hướng
            {
                isMovingRight = false;
            }
            paintbabysharkmove(g, sharkX, sharkY, 0); // Hướng phải
        }
        else
        {
            sharkX -= speed;
            if (sharkX < x) // Khi đến vị trí x thì đổi hướng
            {
                isMovingRight = true;
            }
            paintbabysharkmove(g, sharkX, sharkY, 1); // Hướng trái
        }
    }
    public static void paintbabysharkmove(mGraphics g, int x, int y, int trans)
    {
        int id = GameCanvas.gameTick / 10 % 4;
        if (ModFunc.babyshark[id] != null)
        {
            int imgW = ModFunc.babyshark[id].getWidth() * mGraphics.zoomLevel / 4;
            int imgH = ModFunc.babyshark[id].getHeight() * mGraphics.zoomLevel / 4;
            g.drawImageScale(ModFunc.babyshark[id], x, y, imgW, imgH, trans);
        }
    }
    // Main render method
    public static void render(mGraphics g, int x, int y, int w)
    {
        paintbabyshark(g, x, y,w);
    }
}