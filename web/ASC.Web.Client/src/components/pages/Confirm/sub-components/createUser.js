import React, { useEffect, useState, useCallback } from "react";
import { withRouter } from "react-router";
import { withTranslation } from "react-i18next";
import styled from "styled-components";
import PropTypes from "prop-types";
import { createUser, signupOAuth } from "@appserver/common/api/people";
import { inject, observer } from "mobx-react";
import Avatar from "@appserver/components/avatar";
import Button from "@appserver/components/button";
import TextInput from "@appserver/components/text-input";
import Text from "@appserver/components/text";
import Link from "@appserver/components/link";
import PasswordInput from "@appserver/components/password-input";
import FieldContainer from "@appserver/components/field-container";
import toastr from "@appserver/components/toast/toastr";
import SocialButton from "@appserver/components/social-button";
import {
  getAuthProviders,
  getCapabilities,
} from "@appserver/common/api/settings";
import { getUserFromConfirm } from "@appserver/common/api/people";
import PageLayout from "@appserver/common/components/PageLayout";
import {
  createPasswordHash,
  getProviderTranslation,
} from "@appserver/common/utils";
import { providersData } from "@appserver/common/constants";
import withLoader from "../withLoader";
import MoreLoginModal from "login/moreLogin";
import AppLoader from "@appserver/common/components/AppLoader";
import EmailInput from "@appserver/components/email-input";
import { smallTablet } from "@appserver/components/utils/device";

export const ButtonsWrapper = styled.div`
  display: flex;
  flex-direction: column;
  width: 320px;

  @media (max-width: 768px) {
    width: 100%;
  }

  .buttonWrapper {
    margin-bottom: 8px;
    width: 100%;
  }
`;

const ConfirmContainer = styled.div`
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 70px;
  align-items: center;
  margin: 80px auto 0 auto;
  max-width: 960px;

  @media (max-width: 768px) {
    grid-template-columns: 1fr;
  }
`;

const GreetingContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: left;
  height: 100%;
  padding-bottom: 32px;

  @media (max-width: 768px) {
    display: ${(props) => !props.isGreetingMode && "none"};
  }

  .greeting-title {
    width: 100%;
    padding-bottom: 32px;
  }

  .greeting-block {
    display: flex;
    flex-direction: row;

    .user-info {
      display: flex;
      flex-direction: column;
      margin-left: 12px;
      justify-content: center;
    }

    .avatar {
      height: 54px;
      width: 54px;
    }
  }

  .tooltip {
    position: relative;
    display: inline-block;
    margin-top: 15px;
  }

  .tooltip .tooltiptext {
    background: #ffffff;
    border: 1px solid #eceef1;
    box-sizing: border-box;
    border-radius: 6px;
    position: absolute;
    padding: 16px;
    width: 100%;
  }
`;

const RegisterContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  height: 100%;

  .or-label {
    margin: 0 8px;
  }

  .more-label {
    padding-top: 18px;
  }

  .line {
    display: flex;
    width: 320px;
    align-items: center;
    color: #eceef1;
    padding-top: 35px;

    @media (max-width: 768px) {
      width: 480px;
    }

    @media (max-width: 414px) {
      width: 311px;
    }
  }

  .line:before,
  .line:after {
    content: "";
    flex-grow: 1;
    background: #eceef1;
    height: 1px;
    font-size: 0px;
    line-height: 0px;
    margin: 0px;
  }

  .auth-form-container {
    margin-top: 32px;
    width: 320px;

    @media (max-width: 768px) {
      margin: 32px 0 0 0;
      width: 100%;
    }
    @media (max-width: 375px) {
      margin: 32px 0 0 0;
      width: 100%;
    }
  }

  .auth-form-fields {
    @media (max-width: 768px) {
      display: ${(props) => props.isGreetingMode && "none"};
    }
  }

  .password-field-wrapper {
    width: 100%;
  }

  .is-greeting-mode-button {
    display: ${(props) => !props.isGreetingMode && "none"};

    @media (min-width: 768px) {
      display: none;
    }
  }
`;

const Confirm = (props) => {
  const { settings, t, greetingTitle, providers, isDesktop } = props;
  const inputRef = React.useRef(null);

  const [moreAuthVisible, setMoreAuthVisible] = useState(false);
  const [ssoLabel, setSsoLabel] = useState("");
  const [ssoUrl, setSsoUrl] = useState("");
  const [email, setEmail] = useState("");
  const [emailValid, setEmailValid] = useState(true);
  const [emailErrorText, setEmailErrorText] = useState("");

  const [password, setPassword] = useState("");
  const [passwordValid, setPasswordValid] = useState(true);

  const [fname, setFname] = useState("");
  const [fnameValid, setFnameValid] = useState(true);
  const [sname, setSname] = useState("");
  const [snameValid, setSnameValid] = useState(true);

  const [isLoading, setIsLoading] = useState(false);

  const [errorText, setErrorText] = useState("");

  const [user, setUser] = useState("");

  const [isLoaded, setIsLoaded] = useState(false);

  const [isGreetingMode, setIsGreetingMode] = useState(true);

  const [isEmailErrorShow, setIsEmailErrorShow] = useState(false);
  const [isPasswordErrorShow, setIsPasswordErrorShow] = useState(false);

  const getSso = async () => {
    const data = await getCapabilities();
    setSsoLabel(data.ssoLabel);
    setSsoUrl(data.ssoUrl);
  };

  const focusInput = () => {
    if (inputRef) {
      inputRef.current.focus();
    }
  };

  useEffect(() => {
    const { isAuthenticated, logout } = props;
    if (isAuthenticated) {
      const path = window.location;
      logout(true, path);
    }
  }, []);

  useEffect(async () => {
    const { linkData } = props;
    const uid = linkData.uid;
    const confirmKey = linkData.confirmHeader;
    const user = await getUserFromConfirm(uid, confirmKey);
    setUser(user);

    window.authCallback = authCallback;

    Promise.all([setProviders(), getSso()]).then(() => {
      setIsLoaded(true);
      focusInput();
    });
  }, []);

  const onSubmit = () => {
    const { defaultPage, linkData, hashSettings } = props;
    const isVisitor = parseInt(linkData.emplType) === 2;

    setIsLoading(true);

    setErrorText("");

    let hasError = false;

    if (!fname.trim()) {
      hasError = true;
      setFnameValid(!hasError);
    }

    if (!sname.trim()) {
      hasError = true;
      setSnameValid(!hasError);
    }

    const emailRegex = "[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{2,}$";
    const validationEmail = new RegExp(emailRegex);

    if (!validationEmail.test(email.trim())) {
      hasError = true;
      setEmailValid(!hasError);
    }

    if (!passwordValid || !password.trim()) {
      hasError = true;
      setPasswordValid(!hasError);
    }

    if (hasError) {
      setIsLoading(false);
      return false;
    }

    const hash = createPasswordHash(password, hashSettings);

    const loginData = {
      userName: email,
      passwordHash: hash,
    };

    const personalData = {
      firstname: fname,
      lastname: sname,
      email: email,
    };

    const registerData = Object.assign(personalData, {
      isVisitor: isVisitor,
    });

    const key = props.linkData.confirmHeader;

    createConfirmUser(registerData, loginData, key)
      .then(() => window.location.replace(defaultPage))
      .catch((error) => {
        console.error("confirm error", error);
        setEmailErrorText(error);
        setEmailValid(false);
        setIsLoading(false);
      });
  };

  const authCallback = (profile) => {
    const { defaultPage } = props;
    const { FirstName, LastName, EMail, Serialized } = profile;

    const signupAccount = {
      EmployeeType: null,
      FirstName: FirstName,
      LastName: LastName,
      Email: EMail,
      PasswordHash: "",
      SerializedProfile: Serialized,
    };

    signupOAuth(signupAccount)
      .then(() => {
        window.location.replace(defaultPage);
      })
      .catch((e) => {
        toastr.error(e);
      });
  };

  const setProviders = async () => {
    const { setProviders } = props;

    try {
      await getAuthProviders().then((providers) => {
        setProviders(providers);
      });
    } catch (e) {
      console.error(e);
    }
  };

  const createConfirmUser = async (registerData, loginData, key) => {
    const { login } = props;

    const data = Object.assign(
      { fromInviteLink: true },
      registerData,
      loginData
    );

    const user = await createUser(data, key);

    const { userName, passwordHash } = loginData;

    const response = await login(userName, passwordHash);

    return user;
  };

  const moreAuthOpen = () => {
    setMoreAuthVisible(true);
  };

  const moreAuthClose = () => {
    setMoreAuthVisible(false);
  };

  const onChangeEmail = (e) => {
    setEmail(e.target.value);
    setIsEmailErrorShow(false);
  };

  const onChangeFname = (e) => {
    setFname(e.target.value);
    setFnameValid(true);
    setErrorText("");
  };

  const onChangeSname = (e) => {
    setSname(e.target.value);
    setSnameValid(true);
    setErrorText("");
  };

  const onChangePassword = (e) => {
    setPassword(e.target.value);
    setErrorText("");
    setIsPasswordErrorShow(false);
  };

  const onKeyPress = (event) => {
    if (event.key === "Enter") {
      onSubmit();
    }
  };

  const onSocialButtonClick = useCallback((e) => {
    const providerName = e.target.dataset.providername;
    const url = e.target.dataset.url;

    const { getOAuthToken, getLoginLink } = props;

    try {
      const tokenGetterWin = isDesktop
        ? (window.location.href = url)
        : window.open(
            url,
            "login",
            "width=800,height=500,status=no,toolbar=no,menubar=no,resizable=yes,scrollbars=no"
          );

      getOAuthToken(tokenGetterWin).then((code) => {
        const token = window.btoa(
          JSON.stringify({
            auth: providerName,
            mode: "popup",
            callback: "authCallback",
          })
        );

        tokenGetterWin.location.href = getLoginLink(token, code);
      });
    } catch (err) {
      console.log(err);
    }
  }, []);

  const providerButtons = () => {
    const providerButtons =
      providers &&
      providers.map((item, index) => {
        if (!providersData[item.provider]) return;
        if (index > 1) return;

        const { icon, label, iconOptions, className } = providersData[
          item.provider
        ];

        return (
          <div className="buttonWrapper" key={`${item.provider}ProviderItem`}>
            <SocialButton
              iconName={icon}
              label={getProviderTranslation(label, t)}
              className={`socialButton ${className ? className : ""}`}
              $iconOptions={iconOptions}
              data-url={item.url}
              data-providername={item.provider}
              onClick={onSocialButtonClick}
            />
          </div>
        );
      });

    return providerButtons;
  };

  const ssoButton = () => {
    return (
      <div className="buttonWrapper">
        <SocialButton
          iconName="/static/images/sso.react.svg"
          className="socialButton"
          label={ssoLabel || getProviderTranslation("sso", t)}
          onClick={() => (window.location.href = ssoUrl)}
        />
      </div>
    );
  };

  const oauthDataExists = () => {
    let existProviders = 0;
    providers && providers.length > 0;
    providers.map((item) => {
      if (!providersData[item.provider]) return;
      existProviders++;
    });

    return !!existProviders;
  };

  const ssoExists = () => {
    if (ssoUrl) return true;
    else return false;
  };

  const onGreetingSubmit = () => {
    setIsGreetingMode(false);
    focusInput();
  };

  const onValidateEmail = (res) => {
    //console.log("onValidateEmail", res);
    setEmailValid(res.isValid);
    setEmailErrorText(res.errors[0]);
  };

  const onValidatePassword = (res) => {
    //console.log("onValidatePassword", res);
    setPasswordValid(res);
  };

  const onBlurEmail = () => {
    setIsEmailErrorShow(true);
  };

  const onBlurPassword = () => {
    setIsPasswordErrorShow(true);
  };

  if (!isLoaded) return <AppLoader />;
  return (
    <ConfirmContainer>
      <GreetingContainer isGreetingMode={isGreetingMode}>
        <Text
          fontSize="23px"
          fontWeight={700}
          textAlign="left"
          className="greeting-title"
        >
          {greetingTitle}
        </Text>

        <div className="greeting-block">
          <Avatar className="avatar" role="user" source={user.avatar} />
          <div className="user-info">
            <Text fontSize="15px" fontWeight={600}>
              {user.firstName} {user.lastName}
            </Text>
            <Text fontSize="12px" fontWeight={600} color="#A3A9AE">
              {user.department}
            </Text>
          </div>
        </div>

        <div className="tooltip">
          <span className="tooltiptext">{t("WelcomeUser")}</span>
        </div>
      </GreetingContainer>

      <RegisterContainer isGreetingMode={isGreetingMode}>
        {ssoExists() && <ButtonsWrapper>{ssoButton()}</ButtonsWrapper>}

        {oauthDataExists() && (
          <>
            <ButtonsWrapper>{providerButtons()}</ButtonsWrapper>
            {providers && providers.length > 2 && (
              <Link
                isHovered
                type="action"
                fontSize="13px"
                fontWeight="600"
                color="#3B72A7"
                className="more-label"
                onClick={moreAuthOpen}
              >
                {t("Common:ShowMore")}
              </Link>
            )}
          </>
        )}

        {(oauthDataExists() || ssoExists()) && (
          <div className="line">
            <Text color="#A3A9AE" className="or-label">
              {t("Or")}
            </Text>
          </div>
        )}

        <form className="auth-form-container">
          <div className="auth-form-fields">
            <FieldContainer
              isVertical={true}
              labelVisible={false}
              hasError={isEmailErrorShow && !emailValid}
              errorMessage={
                emailErrorText
                  ? t(`Common:${emailErrorText}`)
                  : t("Common:RequiredField")
              }
            >
              <EmailInput
                id="login"
                name="login"
                type="email"
                hasError={isEmailErrorShow && !emailValid}
                value={email}
                placeholder={t("Common:Email")}
                size="large"
                scale={true}
                isAutoFocussed={true}
                tabIndex={1}
                isDisabled={isLoading}
                autoComplete="username"
                onChange={onChangeEmail}
                onBlur={onBlurEmail}
                onValidateInput={onValidateEmail}
                forwardedRef={inputRef}
                disallowChar='"'
              />
            </FieldContainer>

            <FieldContainer
              isVertical={true}
              labelVisible={false}
              hasError={!fnameValid}
              errorMessage={errorText ? errorText : t("Common:RequiredField")}
            >
              <TextInput
                id="first-name"
                name="first-name"
                type="text"
                hasError={!fnameValid}
                value={fname}
                placeholder={t("FirstName")}
                size="large"
                scale={true}
                tabIndex={1}
                isDisabled={isLoading}
                onChange={onChangeFname}
                onKeyDown={onKeyPress}
              />
            </FieldContainer>

            <FieldContainer
              isVertical={true}
              labelVisible={false}
              hasError={!snameValid}
              errorMessage={errorText ? errorText : t("Common:RequiredField")}
            >
              <TextInput
                id="last-name"
                name="last-name"
                type="text"
                hasError={!snameValid}
                value={sname}
                placeholder={t("Common:LastName")}
                size="large"
                scale={true}
                tabIndex={1}
                isDisabled={isLoading}
                onChange={onChangeSname}
                onKeyDown={onKeyPress}
              />
            </FieldContainer>

            <FieldContainer
              isVertical={true}
              labelVisible={false}
              hasError={isPasswordErrorShow && !passwordValid}
              errorMessage={errorText ? "" : t("Common:RequiredField")}
            >
              <PasswordInput
                simpleView={false}
                hideNewPasswordButton
                showCopyLink={false}
                passwordSettings={settings}
                id="password"
                inputName="password"
                placeholder={t("Common:Password")}
                type="password"
                hasError={isPasswordErrorShow && !passwordValid}
                inputValue={password}
                size="large"
                scale={true}
                tabIndex={1}
                isDisabled={isLoading}
                autoComplete="current-password"
                onChange={onChangePassword}
                onBlur={onBlurPassword}
                onKeyDown={onKeyPress}
                onValidateInput={onValidatePassword}
                tooltipPasswordTitle={`${t("Common:PasswordLimitMessage")}:`}
                tooltipPasswordLength={`${t("Common:PasswordLimitLength", {
                  fromNumber: settings ? settings.minLength : 8,
                  toNumber: 30,
                })};`}
                generatePasswordTitle={t("Wizard:GeneratePassword")}
              />
            </FieldContainer>

            <Button
              id="submit"
              className="login-button"
              primary
              size="large"
              scale={true}
              label={
                isLoading
                  ? t("Common:LoadingProcessing")
                  : t("LoginRegistryButton")
              }
              tabIndex={1}
              isDisabled={isLoading}
              isLoading={isLoading}
              onClick={onSubmit}
            />
          </div>

          <Button
            id="submit"
            className="login-button is-greeting-mode-button"
            primary
            size="large"
            scale={true}
            label={
              isLoading
                ? t("Common:LoadingProcessing")
                : t("LoginRegistryButton")
            }
            tabIndex={1}
            isDisabled={isLoading}
            isLoading={isLoading}
            onClick={onGreetingSubmit}
          />
        </form>

        <MoreLoginModal
          t={t}
          visible={moreAuthVisible}
          onClose={moreAuthClose}
          providers={providers}
          onSocialLoginClick={onSocialButtonClick}
          ssoLabel={ssoLabel}
          ssoUrl={ssoUrl}
        />
      </RegisterContainer>
    </ConfirmContainer>
  );
};

Confirm.propTypes = {
  location: PropTypes.object.isRequired,
  history: PropTypes.object.isRequired,
};
const CreateUserForm = (props) => (
  <PageLayout>
    <PageLayout.SectionBody>
      <Confirm {...props} />
    </PageLayout.SectionBody>
  </PageLayout>
);

export default inject(({ auth }) => {
  const {
    login,
    logout,
    isAuthenticated,
    settingsStore,
    setProviders,
    providers,
    thirdPartyLogin,
  } = auth;
  const {
    passwordSettings,
    greetingSettings,
    hashSettings,
    defaultPage,
    getSettings,
    getPortalPasswordSettings,
    getOAuthToken,
    getLoginLink,
  } = settingsStore;

  return {
    settings: passwordSettings,
    greetingTitle: greetingSettings,
    hashSettings,
    defaultPage,
    isAuthenticated,
    login,
    logout,
    getSettings,
    getPortalPasswordSettings,
    thirdPartyLogin,
    getOAuthToken,
    getLoginLink,
    setProviders,
    providers,
  };
})(
  withRouter(
    withTranslation(["Confirm", "Common", "Wizard"])(
      withLoader(observer(CreateUserForm))
    )
  )
);
