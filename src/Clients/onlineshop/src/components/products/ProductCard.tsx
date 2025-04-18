import { Link } from "react-router-dom";
import { ProductServiceContractsQueriesProductProductResponseItem } from "../../clients/ProductService/types.gen";

type Product = {
    image: string;
} & ProductServiceContractsQueriesProductProductResponseItem;

type ProductCardProps = {
    product: Product;
};

const ProductCard = ({ product }: ProductCardProps) => {
    return (
        <Link
            to={`/products/${product.id}`}
            className="card hover:shadow-lg transition-shadow"
        >
            <div className="h-48 overflow-hidden">
                <img
                    src={product.image}
                    alt={product.name}
                    className="img-cover"
                />
            </div>
            <div className="p-md">
                <h3 className="text-primary text-lg font-semibold mb-sm">
                    {product.name}
                </h3>
                <p className="text-primary font-bold mb-sm">
                    ${product.price?.toFixed(2)}
                </p>
                <button className="btn-primary">Add to Cart</button>
            </div>
        </Link>
    );
};

export default ProductCard;
