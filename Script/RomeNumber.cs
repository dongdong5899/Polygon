public static class RomeNumber
{
    public static string GetNumber(int number)
    {
        string romeNum = "";

        int cExist = number / 100;
        for (int i = 0; i < cExist; i++)
            romeNum += 'C';
        int cNear = number % 100;
        int lExist = cNear / 50;
        if (cNear >= 90)
            romeNum += "XC";
        else
        {
            if (lExist == 1)
                romeNum += 'L';
        }

        int lNear = cNear % 50;
        if (cNear < 90)
        {
            if (lNear >= 40)
                romeNum += "XL";
            else
            {
                int xExist = lNear / 10;
                for (int i = 0; i < xExist; i++)
                    romeNum += 'X';
            }
        }

        int xNear = lNear % 10;
        if (xNear == 9)
            romeNum += "IX";
        else
        {
            int vExist = xNear / 5;
            if (vExist == 1)
                romeNum += 'V';
            int vNear = xNear % 5;
            if (vNear == 4)
                romeNum += "IV";
            else
            {
                for (int i = 0; i < vNear; i++)
                {
                    romeNum += "I";
                }
            }
        }
        return romeNum;
    }
}
