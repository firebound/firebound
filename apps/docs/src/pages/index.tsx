import React from 'react';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import { cn } from '@site/src/helpers/cn';
import Layout from '@theme/Layout';
import Heading from '@theme/Heading';
import HomepageFeatures from '@site/src/components/HomepageFeatures';

import Brand from '@site/public/img/brand.svg';

function HomeHeader() {
    const { siteConfig } = useDocusaurusContext();
    return (
        <div className="flex flex-col max-w-5xl px-2 items-center justify-center w-full h-full text-center">
            <Brand className="w-full max-w-80" />
            <p className="pt-4">
                Framework para criação de jogos no Godot.
            </p>
        </div>
    );
}

export default function Home(): JSX.Element {
    const { siteConfig } = useDocusaurusContext();
    return (
        <Layout
            title={`${siteConfig.title}`}
            description=""
            wrapperClassName="flex flex-col w-full h-full px-2 py-8 space-y-4 items-center justify-center"
        >
            <HomeHeader />
            <main className="flex flex-col w-full max-w-5xl px-2">
                <HomepageFeatures />
            </main>
        </Layout>
    );
}
