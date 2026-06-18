import React, { useEffect, useRef, useState } from "react";
import Panzoom from "@panzoom/panzoom";
import styles from "./styles.module.css";

const ZoomableDiagram = ({ children }: any) => {
    const contentRef = useRef<HTMLDivElement | null>(null);
    const zoomInRef = useRef<HTMLButtonElement | null>(null);
    const zoomOutRef = useRef<HTMLButtonElement | null>(null);
    const resetRef = useRef<HTMLButtonElement | null>(null);
    const [isFullscreen, setIsFullscreen] = useState(false);

    useEffect(() => {
        if (contentRef.current) {
            const container = contentRef.current.parentElement;
            const content = contentRef.current;

            if (container && content) {
                const containerRect = container.getBoundingClientRect();
                const contentRect = content.getBoundingClientRect();

                const scaleX = containerRect.width / contentRect.width;
                const scaleY = containerRect.height / contentRect.height;
                const initialScale = Math.min(scaleX * 2, scaleY * 2);

                const panzoom = Panzoom(content, {
                    maxScale: 200,
                    minScale: 1,
                    startScale: initialScale,
                });

                panzoom.zoom(initialScale);

                // Adicionando ouvintes para os controles de zoom
                if (zoomInRef.current) {
                    zoomInRef.current.addEventListener("click", () => panzoom.zoomIn());
                }
                if (zoomOutRef.current) {
                    zoomOutRef.current.addEventListener("click", () => panzoom.zoomOut());
                }
                if (resetRef.current) {
                    resetRef.current.addEventListener("click", () => {
                        panzoom.reset();
                        panzoom.zoom(initialScale);
                    });
                }

                // Adicionando o controle de zoom com scroll
                const handleScroll = (e: WheelEvent) => {
                    e.preventDefault();
                    if (e.deltaY < 0) {
                        panzoom.zoomIn();
                    } else {
                        panzoom.zoomOut();
                    }
                };

                content.addEventListener("wheel", handleScroll, { passive: false });

                // Cleanup: removendo os ouvintes de eventos quando o componente desmontar
                return () => {
                    if (zoomInRef.current) {
                        zoomInRef.current.removeEventListener("click", () => panzoom.zoomIn());
                    }
                    if (zoomOutRef.current) {
                        zoomOutRef.current.removeEventListener("click", () => panzoom.zoomOut());
                    }
                    if (resetRef.current) {
                        resetRef.current.removeEventListener("click", () => {
                            panzoom.reset();
                            panzoom.zoom(initialScale);
                        });
                    }
                    if (content) {
                        content.removeEventListener("wheel", handleScroll);
                    }
                };
            }
        }
    }, []);

    useEffect(() => {
        const handleEscKey = (event: KeyboardEvent) => {
            if (event.key === 'Escape') {
                setIsFullscreen(false);
            }
        };

        window.addEventListener('keydown', handleEscKey);

        return () => {
            window.removeEventListener('keydown', handleEscKey);
        };
    }, []);

    const toggleFullscreen = () => {
        setIsFullscreen(!isFullscreen);
    };

    return (
        <>
            <div className={`${styles.container} ${isFullscreen ? styles.fullscreen : ""}`}>
                <div ref={contentRef} className={`${styles.content} ${isFullscreen ? styles.fullscreenContent : ""}`}>
                    {children}
                </div>
                <div className={styles.controls}>
                    <button ref={zoomInRef} aria-label="Aumentar zoom">+</button>
                    <button ref={zoomOutRef} aria-label="Diminuir zoom">−</button>
                    <button ref={resetRef} aria-label="Resetar zoom">Reset</button>
                    <button onClick={toggleFullscreen} aria-label="Toggle Fullscreen">
                        {isFullscreen ? "Exit Fullscreen" : "Fullscreen"}
                    </button>
                </div>
            </div>
            {isFullscreen && <div className={styles.overlay} onClick={toggleFullscreen}></div>}
        </>
    );
};

export default ZoomableDiagram;