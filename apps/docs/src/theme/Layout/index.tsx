import React from 'react';
import Layout from '@theme-original/Layout';
import OpenGithubIssueButton from '@site/src/components/OpenGithubIssueButton';

export default function LayoutWrapper(props: any) {
  return (
    <>
      <Layout {...props} />
      <OpenGithubIssueButton />
    </>
  );
}