import React from 'react';
import clsx from 'clsx';
import Heading from '@theme/Heading';
import styles from './styles.module.css';

import Leaf from '@site/public/img/misc/folha_3.svg';

type FeatureItem = {
    title: string;
    Svg?: React.ComponentType<React.ComponentProps<'svg'>>;
    description: JSX.Element;
};

const FeatureList: FeatureItem[] = [
    {
        title: 'Open Source',
        description: (
            <>
                <p className=''>
                    Desenvolvida para apoiar desenvolvedores solo, <i>hobbyistas</i> e estudantes!
                </p>
                <p>
                    Todo o código está disponível no GitHub, permitindo que qualquer pessoa contribua, estude ou utilize o projeto em seus próprios projetos.
                </p>
            </>
        ),
    },
    {
        title: 'Flexível e Modular',
        description: (
            <>
                <p>
                    Criada para ser uma framework modular, a Firebound permite que você escolha os componentes que deseja usar.
                </p>
            </>
        ),
    },
    {
        title: 'Comunidade Primeiro',
        description: (
            <>
                <p>
                    Acreditamos que a colaboração é a chave para o sucesso. Vamos construir juntos uma comunidade é acolhedora e sempre disposta a ajudar!
                </p>

            </>
        ),
    },
];

function Feature({ title, description, Svg = null }: FeatureItem) {
    return (
        <div className={clsx('relative flex flex-col basis-0 flex-1')}>
            <div className="">
                <Leaf className="absolute -top-4 -left-2 w-4" />
                <Leaf className="absolute -top-4 -right-2 w-4 -scale-x-100"/>
            </div>
            {Svg && <div className="">
                <Svg role="img" />
            </div>}
            <div className="z-10 flex flex-col h-full  p-4 rounded-md bg-firebound-beige-100 dark:bg-firebound-brown-800 border-2 border-firebound-beige-300 dark:border-firebound-brown-900">
                <h3>{title}</h3>
                <div className='flex flex-col space-y-4'>{description}</div>
            </div>
        </div>
    );
}

export default function HomepageFeatures(): JSX.Element {
    return (
        <section className="flex flex-col md:flex-row w-full gap-8 pt-4">
            {FeatureList.map((props, idx) => (
                <Feature key={idx} {...props} />
            ))}
        </section>
    );
}