import React, { Component } from "react";
import PropTypes from "prop-types";
import { PageLayout, Loader } from "asc-web-components";

export class ExternalRedirect extends Component {
  constructor(props) {
    super(props);
  }

  componentDidMount() {
    const { to } = this.props;
    to && window.location.replace(to);
  }

  render() {
    return (
      <PageLayout
        sectionBodyContent={
          <Loader className="pageLoader" type="rombs" size='40px' />
        }
      />
    );
  }
}

ExternalRedirect.propTypes = {
  to: PropTypes.string
};

export default ExternalRedirect;
