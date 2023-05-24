import React from "react";
import { useTranslation } from "react-i18next";
import { Link } from "react-router-dom";

function Footer() {
  const { t } = useTranslation();
  return (
    <div className="w-full text-gray-300 px-5 flex flex-col bg-[#001219] h-[15rem]">
      <div className="flex flex-row basis-2/3 pt-10 pb-2">
        <Link
          to="/"
          className="basis-1/5 text-lg xl:text-[2rem] font-bold text-gray-100 mr-[2.3rem]"
        >
          bookcamp
        </Link>
        <div className="basis-1/5 text-xs xl:text-base flex flex-col mr-[11.6rem]">
          <p>{t("footerLine1")!}</p>
          <p>{t("footerLine2")!}</p>
          <p>{t("footerLine3")!}</p>
          <p>{t("footerLine4")!}</p>
        </div>
        <div className="basis-1/5 text-xs xl:text-base flex flex-col">
          <p>{t("footerLine5")!}</p>
          <p>{t("footerLine6")!}</p>
        </div>
        <div className="basis-1/5"></div>
      </div>
      <div className="flex flex-row font-extralight items-center h-20 justify-start border-t-2 border-[#313131]">
        <p className="text-xs xl:text-sm text-left w-[21rem]">
          2022. {t("footerLine7")!}
        </p>
        <div className="flex flex-row justify-center text-xs xl:text-sm">
          <p>{t("footerLine8")!}</p>
          <p className="px-10">{t("footerLine9")!}</p>
          <p>{t("footerLine10")!}</p>
        </div>
      </div>
    </div>
  );
}

export default Footer;
