import { Link } from "react-router-dom";
import { ProductServiceContractsQueriesProductProductResponseItem } from "../../clients/ProductService/types.gen";
import { useState } from "react";

type Product = {
    image: string;
} & ProductServiceContractsQueriesProductProductResponseItem;

type ProductCardProps = {
    product: Product;
};

const ProductCard = ({ product }: ProductCardProps) => {
    const [isHovered, setIsHovered] = useState(false);

    return (
        <div
            className="card transition-all duration-300 h-full flex flex-col"
            onMouseEnter={() => setIsHovered(true)}
            onMouseLeave={() => setIsHovered(false)}
        >
            {/* Product image with hover effect */}
            <Link
                to={`/products/${product.id}`}
                className="block relative overflow-hidden aspect-square"
            >
                <div className="absolute inset-0 bg-gray-100 animate-pulse"></div>
                <img
                    src={product.image}
                    alt={product.name || "Product"}
                    className="object-cover w-full h-full absolute inset-0 transition-transform duration-500 ease-out"
                    style={{
                        transform: isHovered ? "scale(1.05)" : "scale(1)",
                    }}
                    loading="lazy"
                />
            </Link>

            {/* Product details */}
            <div className="p-4 flex-grow flex flex-col">
                <Link to={`/products/${product.id}`} className="no-underline">
                    <h3 className="text-lg font-medium text-text mb-1 truncate hover:text-primary transition-colors">
                        {product.name}
                    </h3>
                </Link>

                <div className="text-xl font-semibold text-primary mb-3">
                    ${product.price?.toFixed(2)}
                </div>

                {/* Add to cart button - grows to fill available space */}
                <div className="mt-auto">
                    <button
                        className="btn btn-primary w-full transition-all duration-300 rounded-md flex items-center justify-center"
                        style={{
                            transform: isHovered
                                ? "translateY(0)"
                                : "translateY(3px)",
                            opacity: isHovered ? 1 : 0.95,
                        }}
                    >
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            className="h-5 w-5 mr-2"
                            fill="none"
                            viewBox="0 0 24 24"
                            stroke="currentColor"
                        >
                            <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                strokeWidth={2}
                                d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z"
                            />
                        </svg>
                        Add to Cart
                    </button>
                </div>
            </div>
        </div>
    );
};

export default ProductCard;
