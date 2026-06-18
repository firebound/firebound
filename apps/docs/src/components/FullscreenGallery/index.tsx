import React, { useState, useRef, useEffect, useCallback } from "react";
import styles from "./styles.module.css";

interface ImageData {
    image: string;
    title?: string;
}

interface FullScreenGalleryProps {
    images: ImageData[];
}

const FullScreenGallery: React.FC<FullScreenGalleryProps> = ({ images }) => {
    const [currentIndex, setCurrentIndex] = useState(0);
    const [isOpen, setIsOpen] = useState(false);
    const [selectedThumbnail, setSelectedThumbnail] = useState<number | null>(null);
    const [isScrollable, setIsScrollable] = useState(false);

    const thumbnailWrapperRef = useRef<HTMLDivElement | null>(null);

    const openFullscreen = (index: number) => {
        setCurrentIndex(index);
        setIsOpen(true);
        setSelectedThumbnail(index);
    };

    const closeFullscreen = () => {
        setIsOpen(false);
        setSelectedThumbnail(null);
    };

    const goToNext = useCallback((e: React.MouseEvent) => {
        e.stopPropagation();

        setCurrentIndex((prevIndex) => {
            const nextIndex = (prevIndex + 1) % images.length;
            setSelectedThumbnail(nextIndex);
            return nextIndex;
        });
    }, [images.length]);

    const goToPrevious = useCallback((e: React.MouseEvent) => {
        e.stopPropagation();

        setCurrentIndex((prevIndex) => {
            const prevIndexFinal = prevIndex === 0 ? images.length - 1 : prevIndex - 1;
            setSelectedThumbnail(prevIndexFinal);
            return prevIndexFinal;
        });
    }, [images.length]);

    useEffect(() => {
        if (thumbnailWrapperRef.current) {
            setIsScrollable(thumbnailWrapperRef.current.scrollWidth > thumbnailWrapperRef.current.clientWidth);
        }
    }, [images.length]);

    useEffect(() => {
        if (thumbnailWrapperRef.current && selectedThumbnail !== null) {
            const selectedThumbnailElement = thumbnailWrapperRef.current.children[selectedThumbnail] as HTMLElement;
            if (selectedThumbnailElement) {
                thumbnailWrapperRef.current.scrollTo({
                    left: selectedThumbnailElement.offsetLeft - thumbnailWrapperRef.current.offsetWidth / 2 + selectedThumbnailElement.offsetWidth / 2,
                    behavior: 'smooth',
                });
            }
        }
    }, [selectedThumbnail]);

    useEffect(() => {
        const handleEscKey = (event: KeyboardEvent) => {
            if (event.key === 'Escape') {
                closeFullscreen();
            }
        };

        window.addEventListener('keydown', handleEscKey);

        return () => {
            window.removeEventListener('keydown', handleEscKey);
        };
    }, []);

    return (
        <>
            <div className={styles.galleryContainer}>
                {isScrollable && (
                    <button
                        className={`${styles.navButton} ${styles.navButtonLeft} ${currentIndex === 0 && styles.disabled}`}
                        onClick={goToPrevious}
                        disabled={currentIndex === 0}
                    >
                        {"<"}
                    </button>
                )}

                <div ref={thumbnailWrapperRef} className={styles.thumbnailWrapper}>
                    {images.map((imageData, index) => (
                        <Thumbnail
                            key={index}
                            image={imageData.image}
                            index={index}
                            onClick={openFullscreen}
                            isSelected={selectedThumbnail === index}
                        />
                    ))}
                </div>

                {isScrollable && (
                    <button
                        className={`${styles.navButton} ${styles.navButtonRight} ${currentIndex === images.length - 1 && styles.disabled}`}
                        onClick={goToNext}
                        disabled={currentIndex === images.length - 1}
                    >
                        {">"}
                    </button>
                )}
            </div>

            {isOpen && (
                <div className={styles.fullscreenOverlay} onClick={closeFullscreen}>
                    <div className={styles.fullscreenImageContainer}>
                        <img
                            className={styles.fullscreenImage}
                            src={images[currentIndex].image}
                            alt={images[currentIndex].title || `Imagem ${currentIndex + 1} de ${images.length}`}
                        />
                        {images[currentIndex].title && (
                            <div className={styles.fullscreenTitle}>
                                {images[currentIndex].title}
                            </div>
                        )}
                        <button
                            className={`${styles.navButton} ${styles.navButtonLeft} ${styles.navButtonFullscreen}`}
                            onClick={goToPrevious}
                            style={{ cursor: currentIndex === 0 ? 'not-allowed' : 'pointer' }}
                        >
                            {"<"}
                        </button>
                        <button
                            className={`${styles.navButton} ${styles.navButtonRight} ${styles.navButtonFullscreen}`}
                            onClick={goToNext}
                            style={{ cursor: currentIndex === images.length - 1 ? 'not-allowed' : 'pointer' }}
                        >
                            {">"}
                        </button>

                        <div className={styles.fullscreenNavigationDots}>
                            {images.map((_, index) => (
                                <div
                                    key={index}
                                    className={`${styles.dot} ${currentIndex === index ? styles.active : ""}`}
                                    onClick={() => setCurrentIndex(index)}
                                />
                            ))}
                        </div>
                    </div>
                </div>
            )}
        </>
    );
};

const Thumbnail = React.memo(
    React.forwardRef<HTMLImageElement, { image: string; index: number; onClick: (index: number) => void; isSelected: boolean }>(
        ({ image, index, onClick, isSelected }, ref) => (
            <img
                ref={ref}
                className={`${styles.thumbnail} ${isSelected ? styles.selectedThumbnail : ""}`}
                src={image}
                alt={`Thumbnail ${index}`}
                onClick={() => onClick(index)}
            />
        )
    )
);

export default FullScreenGallery;